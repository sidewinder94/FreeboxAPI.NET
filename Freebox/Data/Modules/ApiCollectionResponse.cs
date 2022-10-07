using System.Collections.Generic;
using Newtonsoft.Json;

namespace Freebox.Data.Modules;

public class ApiCollectionResponse<T> : ApiResponseBase where T : IFreeboxApiResponse
{
    [JsonProperty("result")]
    public IReadOnlyList<T> Result { get; set; }
}