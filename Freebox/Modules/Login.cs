using Freebox.Data.Modules;
using Freebox.Data.Modules.Login.Requests;
using Freebox.Data.Modules.Login.Responses;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Freebox.Modules
{

    [SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
    public partial class Login : BaseModule
    {
        private const string BaseModuleUri = "v6/login/";

        private readonly FreeboxAPI _freeboxApi;

        internal bool LoggedIn = false;

        internal Login(FreeboxAPI api)
        {
            this._freeboxApi = api;
        }

        public async Task<ApiResponse<AuthorizeResponse>> Authorize(AuthorizeCreationRequest request)
        {
            var uri = new Uri($"{this._freeboxApi.ApiInfo.ApiUri}{BaseModuleUri}authorize/");

            return await PostAsync<AuthorizeCreationRequest, AuthorizeResponse>(request, uri);
        }

        public async Task<ApiResponse<AuthorizeProgressResponse>> TrackAuthorization(AuthorizeResponse authorizeResponse)
        {
            if (authorizeResponse == null)
            {
                throw new ArgumentNullException(
                    nameof(authorizeResponse), 
                    "Cannot track the authorization request without at least an" + 
                        $" Id please provide a {typeof(AuthorizeResponse).Name} object with at least the {nameof(AuthorizeResponse.TrackId)} property filled");
            }

            var uri = new Uri($"{_freeboxApi.ApiInfo.ApiUri}{BaseModuleUri}authorize/{authorizeResponse.TrackId}");
            return await GetAsync<AuthorizeProgressResponse>(uri);
        }
    }
}
