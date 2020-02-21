﻿using Freebox.Data;
using Freebox.Modules;
using Freebox.Parser;
using Makaretu.Dns;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Freebox
{
    public class FreeboxAPI
    {
        private readonly Lazy<Login> _login;

        public ApiInfo ApiInfo { get; private set; }

        public Login Login { get => this._login.Value; }


        private FreeboxAPI()
        {
            this._login = new Lazy<Login>(() => new Login(this));
        }

        /// <summary>
        /// Method allowing to discover a Freebox on the network
        /// </summary>
        /// <returns></returns>
        public static async Task<FreeboxAPI> GetFreeboxApiInstance(CancellationToken ct)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            var result = new FreeboxAPI();

            using (var mdns = new MulticastService())
            using(var sd = new ServiceDiscovery(mdns))
            {
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
    }
}