using Freebox.Certificates;
using Freebox.Data.Modules;
using Freebox.Exceptions;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Freebox.Modules;

/// <summary>
/// Base module class, used to regroup common module code (e.g. http requests)
/// </summary>
public abstract class BaseModule
{
    protected BaseModule(FreeboxApi api)
    {
        this.FreeboxApi = api;
    }

    protected FreeboxApi FreeboxApi { get; }

    protected async Task<ApiResponse<TRes>> PostAsync<TReq, TRes>(TReq request, Uri uri, bool bypassAutoLogin = false) where TRes : IFreeboxApiResponse
    {
        using var handler = new WinHttpHandler();
        handler.ServerCertificateValidationCallback = CertificateHelper.ValidateCertificate;

        using var httpClient = await this.SetHttpClientHeaders(new HttpClient(handler), bypassAutoLogin);
        using var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        
        var response = await httpClient.PostAsync(uri, content);

        var result = JsonConvert.DeserializeObject<ApiResponse<TRes>>(await response.Content.ReadAsStringAsync());

        if (response.IsSuccessStatusCode)
        {
            return result;
        }

        throw new FreeboxException(result, response);
    }
    
    protected async Task<ApiResponse<TRes>> PutAsync<TReq, TRes>(TReq request, Uri uri, bool bypassAutoLogin = false) where TRes : IFreeboxApiResponse
    {
        using var handler = new WinHttpHandler();
        handler.ServerCertificateValidationCallback = CertificateHelper.ValidateCertificate;

        using var httpClient = await this.SetHttpClientHeaders(new HttpClient(handler), bypassAutoLogin);
        using var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        
        var response = await httpClient.PutAsync(uri, content);

        var result = JsonConvert.DeserializeObject<ApiResponse<TRes>>(await response.Content.ReadAsStringAsync());

        if (response.IsSuccessStatusCode)
        {
            return result;
        }

        throw new FreeboxException(result, response);
    }

    protected async Task<ApiResponse<TRes>> GetAsync<TRes>(Uri uri, object request = null,bool bypassAutoLogin = false) where TRes : IFreeboxApiResponse
    {
        var response = await this.InternalGetAsync(uri, request, bypassAutoLogin);
        return await PrepareResponse<TRes>(response);
    }

    protected async Task<ApiCollectionResponse<TRes>> GetCollectionAsync<TRes>(Uri uri, object request = null,
        bool bypassAutoLogin = false) where TRes : IFreeboxApiResponse
    {
        var response = await this.InternalGetAsync(uri, request, bypassAutoLogin);
        return await PrepareCollectionResponse<TRes>(response);
    }

    private async Task<HttpResponseMessage> InternalGetAsync(Uri uri, object request = null,bool bypassAutoLogin = false)
    {
        using var handler = new WinHttpHandler();
        
        handler.ServerCertificateValidationCallback = CertificateHelper.ValidateCertificate;

        using var httpClient = await this.SetHttpClientHeaders(new HttpClient(handler), bypassAutoLogin);
        using var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        using var preparedRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = uri
        };
        
        if(request != null)
        {
            preparedRequest.Content = content;
        }

        return await httpClient.SendAsync(preparedRequest);
    }

    private static async Task<ApiResponse<TRes>> PrepareResponse<TRes>(HttpResponseMessage response) where TRes : IFreeboxApiResponse
    {
        var result = JsonConvert.DeserializeObject<ApiResponse<TRes>>(await response.Content.ReadAsStringAsync());

        if (response.IsSuccessStatusCode)
        {
            return result;
        }

        throw new FreeboxException(result, response);
    }
    
    private static async Task<ApiCollectionResponse<TRes>> PrepareCollectionResponse<TRes>(HttpResponseMessage response) where TRes : IFreeboxApiResponse
    {
        var result = JsonConvert.DeserializeObject<ApiCollectionResponse<TRes>>(await response.Content.ReadAsStringAsync());

        if (response.IsSuccessStatusCode)
        {
            return result;
        }

        throw new FreeboxException(result, response);
    }

    /// <summary>
    /// This method automatically sets the authentication headers, and makes sure we are logged in unless explicitely stated by the module method
    /// </summary>
    /// <param name="client">the client to configure</param>
    /// <param name="bypassAutoLogin">A value indicating whether we should bypass the auto-login mechanism</param>
    /// <returns></returns>
    private async Task<HttpClient> SetHttpClientHeaders(HttpClient client, bool bypassAutoLogin)
    {
        // If we're not logged in
        if (string.IsNullOrWhiteSpace(this.FreeboxApi.Login.SessionToken))
        {
            // But we do have an application token
            if(string.IsNullOrWhiteSpace(this.FreeboxApi.AppInfo.AppToken))
            {
                throw new FreeboxException(LocalizedStrings.AuthentificationExceptionNotLogged);
            }

            // We try to log in automatically if the caller didn't explicitly disable the behaviour (the login method for example, we'd fall into a stackoverflow)
            if (!bypassAutoLogin)
            {
                await this.FreeboxApi.Login.SessionOpen();
            }
        }

        client.DefaultRequestHeaders.Add("X-Fbx-App-Auth", this.FreeboxApi.Login.SessionToken);

        return client;
    }
}