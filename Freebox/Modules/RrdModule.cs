using Freebox.Data.Modules;
using Freebox.Data.Modules.RRD.Requests;
using Freebox.Data.Modules.RRD.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Freebox.Modules;

public class RrdModule : BaseModule
{
    private const string BaseModuleUri = "v5/rrd/";

    internal RrdModule(FreeboxApi api) : base(api)
    {
    }

    public async Task<ApiResponse<RrdResponse<T>>> GetRrd<T>(Fetch<T> fetch) where T : RrdResponseDbIdent, new()
    {
        return await this.PostAsync<Fetch<T>, RrdResponse<T>>(fetch,
            new Uri($"{this.FreeboxApi.ApiInfo.ApiUri}{BaseModuleUri}"));
    }

    public async IAsyncEnumerable<ApiResponse<RrdResponse<T>>> StreamRrd<T>(
        RrdFields? fields,
        [EnumeratorCancellation] CancellationToken cancellationToken, 
        TimeSpan? customQueryDelay = null)
        where T : RrdResponseDbIdent, new()
    {
        var fetch = new Fetch<T>
        {
            Fields = fields,
            DateStart = DateTime.Now.AddSeconds(-30),
            DateEnd = DateTime.Now
        };

        var queryDelay =
            (int)(customQueryDelay is { TotalSeconds: > 30 } ? customQueryDelay.Value : new TimeSpan(0, 0, 30))
            .TotalMilliseconds;

        while (!cancellationToken.IsCancellationRequested)
        {
            yield return await this.GetRrd(fetch);

            await Task.Delay(queryDelay, cancellationToken);

            Debug.WriteLine($"Fetch start {fetch.DateStart} ; end {fetch.DateEnd}");

            fetch.DateStart = fetch.DateEnd.Value;
            fetch.DateEnd = DateTime.Now;
        }
    }
}