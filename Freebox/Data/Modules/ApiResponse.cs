using Newtonsoft.Json;

namespace Freebox.Data.Modules;

/// <summary>
/// Common response properties, endpoint specific data is stored in <see cref="Result"/>
/// </summary>
/// <typeparam name="T">Type of the response, must "implement" <see cref="IFreeboxApiResponse"/></typeparam>
public class ApiResponse<T> : ApiResponseBase where T : IFreeboxApiResponse
{
    [JsonProperty("result")]
    public T Result { get; set; }
}