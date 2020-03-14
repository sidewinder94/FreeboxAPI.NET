using Freebox.Data.Modules;
using Freebox.Data.Modules.RRD.Requests;
using Freebox.Data.Modules.RRD.Responses;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Freebox.Modules
{
    [SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
    public class RRD : BaseModule
    {
        private const string BaseModuleUri = "v5/rrd/";

        internal RRD(FreeboxAPI api) : base(api)
        {
        }

        public async Task<ApiResponse<RrdResponse>> GetRrd(Fetch fetch)
        {
            return await this.PostAsync<Fetch,RrdResponse>(fetch, new Uri($"{FreeboxApi.ApiInfo.ApiUri}{BaseModuleUri}"));
        }

    }
}
