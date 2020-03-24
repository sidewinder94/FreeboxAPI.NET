using Freebox.Data.Modules;
using Freebox.Data.Modules.RRD.Requests;
using Freebox.Data.Modules.RRD.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
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
            return await this.PostAsync<Fetch<T>, RrdResponse<T>>(fetch, new Uri($"{FreeboxApi.ApiInfo.ApiUri}{BaseModuleUri}"));
        }

        public async IAsyncEnumerable<ApiResponse<RrdResponse<T>>> StreamRrd<T>(RrdFields? fields, CancellationToken cancellationToken, TimeSpan? customQueryDelay = null) where T : RrdResponseDbIdent, new()
        {
            var fetch = new Fetch<T>
            {
                Fields = fields
            };

            if (!fetch.DateStart.HasValue)
            {
                fetch.DateStart = DateTime.Now.AddSeconds(-30);
                fetch.DateEnd = DateTime.Now;

            }

            int queryDelay = (int)(customQueryDelay.HasValue && customQueryDelay.Value.TotalSeconds > 30 ? customQueryDelay.Value : new TimeSpan(0, 0, 30)).TotalMilliseconds;

            while (!cancellationToken.IsCancellationRequested)
            {
                yield return await this.GetRrd<T>(fetch);

                await Task.Delay(queryDelay);

                Debug.WriteLine($"Fetch start {fetch.DateStart} ; end {fetch.DateEnd}");

                fetch.DateStart = fetch.DateEnd.Value;
                fetch.DateEnd = DateTime.Now;
            }
        }

    }
}
