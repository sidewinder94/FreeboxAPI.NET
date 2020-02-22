using Freebox.Data.Modules;
using Freebox.Data.Modules.Login.Requests;
using Freebox.Data.Modules.Login.Responses;
using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;
using Freebox.Data.Modules.Login;

namespace Freebox.Modules
{

    [SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
    public partial class Login : BaseModule
    {
        private const string BaseModuleUri = "v6/login/";

        private readonly FreeboxAPI _freeboxApi;

        public bool LoggedIn { get; internal set; } = false;

        public Permissions Permissions { get; internal set; } = null;

        internal Login(FreeboxAPI api)
        {
            this._freeboxApi = api;
        }

        public async Task<ApiResponse<AuthorizeResponse>> Authorize()
        {
            var request = new AuthorizeCreationRequest()
            {
                AppId = _freeboxApi.AppInfo.AppId,
                AppName = _freeboxApi.AppInfo.AppName,
                AppVersion = _freeboxApi.AppInfo.AppVersion,
                DeviceName = _freeboxApi.AppInfo.DeviceName
            };

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

        public async Task<ApiResponse<AuthorizeProgressResponse>> WaitForAuthorization(AuthorizeResponse authorizeResponse, CancellationToken ct)
        {
            var authorizeTrack = await this.TrackAuthorization(authorizeResponse);

            while (authorizeTrack.Result.Status == AuthorizeStatus.Pending && !ct.IsCancellationRequested)
            {
                authorizeTrack = await this.TrackAuthorization(authorizeResponse);

                await Task.Delay(200);
            }

            return authorizeTrack;
        }

        public async Task<ApiResponse<OpenedSession>> SessionStart(AuthorizeResponse authorizeResponse)
        {
            if (authorizeResponse == null)
            {
                throw new ArgumentNullException(nameof(authorizeResponse));
            }

            return await this.SessionStart(authorizeResponse.AppToken);
        }

        [SuppressMessage("Security", "CA5350:Ne pas utiliser d'algorithmes de chiffrement faibles", Justification = "Imposed by the documentation")]
        public async Task<ApiResponse<OpenedSession>> SessionStart(string appSecret)
        {
            if (string.IsNullOrWhiteSpace(appSecret))
            {
                throw new ArgumentNullException(nameof(appSecret));
            }

            var uri = new Uri($"{_freeboxApi.ApiInfo.ApiUri}{BaseModuleUri}");

            var challengeResponse = await GetAsync<LoginStart>(uri);

            using (var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(appSecret)))
            {
                var challengeToReturn = hmac.ComputeHash(Encoding.UTF8.GetBytes(challengeResponse.Result.Challenge)).Aggregate("", (s, e) => s + string.Format(CultureInfo.InvariantCulture, "{0:x2}", e), s => s);

                var sessionStart = new SessionStart()
                {
                    AppId = this._freeboxApi.AppInfo.AppId,
                    Password = challengeToReturn
                };

                var response = await PostAsync<SessionStart, OpenedSession>(sessionStart, new Uri($"{uri}session/"));

                this.LoggedIn = response.Success;

                this.Permissions = response.Result.Permissions;

                return response;
            }
        }
    }
}
