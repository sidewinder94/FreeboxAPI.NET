using Freebox.Certificates;
using Freebox.Data.Modules;
using Freebox.Exceptions;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Freebox.Modules
{
    /// <summary>
    /// Base module class, used to regroup common module code (e.g. http requests)
    /// </summary>
    public abstract class BaseModule
    {
        protected static async Task<ApiResponse<TRes>> PostAsync<TReq, TRes>(TReq request, Uri uri) where TRes : IFreeboxApiResponse
        {
            using (var handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback = CertificateHelper.ValidateCertificate;

                using (var httpClient = new HttpClient(handler))
                using (var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"))
                {

                    var response = await httpClient.PostAsync(uri, content);

                    var result = JsonConvert.DeserializeObject<ApiResponse<TRes>>(await response.Content.ReadAsStringAsync());

                    if (response.IsSuccessStatusCode)
                    {
                        return result;
                    }
                    else
                    {
                        throw new FreeboxException(result, response);
                    }
                }
            }
        }
        protected static async Task<ApiResponse<TRes>> GetAsync<TRes>(Uri uri) where TRes : IFreeboxApiResponse
        {
            using (var handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback = CertificateHelper.ValidateCertificate;

                using (var httpClient = new HttpClient(handler))
                {
                    var response = await httpClient.GetAsync(uri);

                    return await PrepareResponse<TRes>(response);
                }
            }
        }

        private static async Task<ApiResponse<TRes>> PrepareResponse<TRes>(HttpResponseMessage response) where TRes : IFreeboxApiResponse
        {
            var result = JsonConvert.DeserializeObject<ApiResponse<TRes>>(await response.Content.ReadAsStringAsync());

            if (response.IsSuccessStatusCode)
            {
                return result;
            }
            else
            {
                throw new FreeboxException(result, response);
            }
        }
    }
}
