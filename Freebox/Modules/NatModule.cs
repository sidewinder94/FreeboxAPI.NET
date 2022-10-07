using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Freebox.Data.Modules;
using Freebox.Data.Modules.Nat.Responses;

namespace Freebox.Modules;

public class NatModule : BaseModule
{
    // ReSharper disable StringLiteralTypo
    private const string BaseModuleUri = "v4/fw/";
    private readonly string BasePortForwardingUri = $"{BaseModuleUri}redir/";
    // ReSharper restore StringLiteralTypo
    
    /// <inheritdoc />
    public NatModule(FreeboxApi api) : base(api)
    {
    }

    public async Task<ApiCollectionResponse<PortForwardingResponse>> GetAllPortForwarding()
    {
        var result =
            await this.GetCollectionAsync<PortForwardingResponse>(
                new Uri($"{this.FreeboxApi.ApiInfo.ApiUri}{this.BasePortForwardingUri}"));

        return result;
    }
    
    public async Task<ApiResponse<PortForwardingResponse>> GetPortForwarding(int id)
    {
        var result =
            await this.GetAsync<PortForwardingResponse>(
                new Uri($"{this.FreeboxApi.ApiInfo.ApiUri}{this.BasePortForwardingUri}{id}"));

        return result;
    }

    public async Task<ApiResponse<PortForwardingResponse>> UpdatePortForwarding(PortForwardingResponse portForwarding)
    {
        if (portForwarding == null) throw new ArgumentNullException(nameof(portForwarding));
        
        return await this.PutAsync<PortForwardingResponse, PortForwardingResponse>(portForwarding,
            new Uri($"{this.FreeboxApi.ApiInfo.ApiUri}{this.BasePortForwardingUri}{portForwarding.Id}"));
    }
}