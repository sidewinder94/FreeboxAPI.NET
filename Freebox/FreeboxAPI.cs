using Freebox.Data;
using Freebox.Modules;
using Freebox.Parser;
using Makaretu.Dns;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Freebox;

public class FreeboxApi
{
    private readonly Lazy<LoginModule> _login;

    private readonly Lazy<RrdModule> _rrd;

    private readonly Lazy<NatModule> _nat;

    public ApiInfo ApiInfo { get; private set; }

    public LoginModule Login => this._login.Value;

    public RrdModule Rrd => this._rrd.Value;

    public NatModule Nat => this._nat.Value;

    public AppInfo AppInfo { get; private set; }

    private FreeboxApi()
    {
        this._login = new Lazy<LoginModule>(() => new LoginModule(this));
        this._rrd = new Lazy<RrdModule>(() => new RrdModule(this));
        this._nat = new Lazy<NatModule>(() => new NatModule(this));
    }

    [SuppressMessage("Design", "CA1031:Ne pas intercepter les types d\'exception générale")]
    ~FreeboxApi()
    {
        // If we are logged in and the api object is destructing, we do try to log out.
        if (!this.Login.LoggedIn) return;
            
        try
        {
            _ = this.Login.SessionClose().Result;
        }
        // ReSharper disable once EmptyGeneralCatchClause
        catch
        { }
    }

    /// <summary>
    /// Method allowing to discover a Freebox on the network
    /// </summary>
    /// <returns></returns>
    public static async Task<FreeboxApi> GetFreeboxApiInstance(AppInfo appInfo, CancellationToken ct)
    {
        ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

        var result = new FreeboxApi()
        {
            AppInfo = appInfo
        };

        using var mdns = new MulticastService();
        using var sd = new ServiceDiscovery(mdns);
            
        mdns.NetworkInterfaceDiscovered += (s, e) =>
        {
            foreach (var nic in e.NetworkInterfaces)
            {
                Console.WriteLine($"NIC '{nic.Name}'");
            }

            sd.QueryServiceInstances("_fbx-api._tcp");
        };

        sd.ServiceInstanceDiscovered += (s, e) =>
        {
            if (e.ServiceInstanceName.IsSubdomainOf("_fbx-api._tcp.local"))
            {
                TXTRecord txt = e.Message.Answers.FirstOrDefault(a => a.Type == DnsType.TXT) as TXTRecord;

                if (txt != null)
                {
                    result.ApiInfo = txt.ApiInfo();
                }
            }
        };

        try
        {
            mdns.Start();

            while (result.ApiInfo == null && !ct.IsCancellationRequested)
            {
                await Task.Delay(20);
            }

            ct.ThrowIfCancellationRequested();

            return result;
        }
        catch (OperationCanceledException)
        {
            return null;
        }
        finally
        {
            mdns.Stop();
        }
    }
}