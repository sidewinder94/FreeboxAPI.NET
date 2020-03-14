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

        public async Task<ApiResponse<RrdResponse<T>>> GetRrd<T>(Fetch<T> fetch) where T : RrdResponseDbIdent, new()
        {
            return await this.PostAsync<Fetch<T>,RrdResponse<T>>(fetch, new Uri($"{FreeboxApi.ApiInfo.ApiUri}{BaseModuleUri}"));
        }

    }
}
