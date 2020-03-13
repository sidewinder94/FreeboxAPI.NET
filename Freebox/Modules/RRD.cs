using Freebox.Data.Modules;
using Freebox.Data.Modules.RRD.Requests;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Freebox.Modules
{
    [SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
    public class RRD : BaseModule
    {
        private const string BaseModuleUri = "v6/rrd/";

        internal RRD(FreeboxAPI api) : base(api)
        {
        }

        public async Task<ApiResponse<EmptyResponse>> GetRrd(Fetch fetch)
        {
            return await this.PostAsync<Fetch, EmptyResponse>(fetch, new Uri(""));
        }

    }
}
