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

        public bool LoggedIn { get; internal set; } = false;

        public string SessionToken { get; internal set; }

        public FreeboxPermissions Permissions { get; internal set; } = null;

        internal Login(FreeboxAPI api) : base (api)
        {
            
        }

        public async Task<ApiResponse<AuthorizeResponse>> Authorize()
        {
            var request = new AuthorizeCreationRequest()
            {
                AppId = FreeboxApi.AppInfo.AppId,
                AppName = FreeboxApi.AppInfo.AppName,
                AppVersion = FreeboxApi.AppInfo.AppVersion,
                DeviceName = FreeboxApi.AppInfo.DeviceName
            };

            var uri = new Uri($"{this.FreeboxApi.ApiInfo.ApiUri}{BaseModuleUri}authorize/");

            return await PostAsync<AuthorizeCreationRequest, AuthorizeResponse>(request, uri, bypassAutoLogin: true);
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

            var uri = new Uri($"{FreeboxApi.ApiInfo.ApiUri}{BaseModuleUri}authorize/{authorizeResponse.TrackId}");
            return await GetAsync<AuthorizeProgressResponse>(uri, bypassAutoLogin: true);
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

        public async Task<ApiResponse<OpenedSession>> SessionOpen(AuthorizeResponse authorizeResponse)
        {
            if (authorizeResponse == null)
            {
                throw new ArgumentNullException(nameof(authorizeResponse));
            }

            return await this.SessionOpen(authorizeResponse.AppToken);
        }

        [SuppressMessage("Security", "CA5350:Ne pas utiliser d'algorithmes de chiffrement faibles", Justification = "Imposed by the documentation")]
        public async Task<ApiResponse<OpenedSession>> SessionOpen(string appSecret = null)
        {
            if (string.IsNullOrWhiteSpace(appSecret) && string.IsNullOrWhiteSpace(this.FreeboxApi.AppInfo.AppToken))
            {
                throw new ArgumentNullException(nameof(appSecret), LocalizedStrings.AppTokenNotProvided);
            }

            if(!string.IsNullOrWhiteSpace(appSecret) && string.IsNullOrWhiteSpace(this.FreeboxApi.AppInfo.AppToken))
            {
                this.FreeboxApi.AppInfo.AppToken = appSecret;
            }

            var appToken = appSecret ?? this.FreeboxApi.AppInfo.AppToken;

            var uri = new Uri($"{FreeboxApi.ApiInfo.ApiUri}{BaseModuleUri}");

            var challengeResponse = await GetAsync<LoginStart>(uri, bypassAutoLogin: true);

            using (var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(appToken)))
            {
                var challengeToReturn = hmac
                    .ComputeHash(Encoding.UTF8.GetBytes(challengeResponse.Result.Challenge))
                    .Aggregate("", (s, e) => s + string.Format(CultureInfo.InvariantCulture, "{0:x2}", e), s => s);

                var sessionStart = new SessionStart()
                {
                    AppId = this.FreeboxApi.AppInfo.AppId,
                    Password = challengeToReturn
                };

                var response = await PostAsync<SessionStart, OpenedSession>(sessionStart, new Uri($"{uri}session/"), bypassAutoLogin: true);

                this.LoggedIn = response.Success;

                this.SessionToken = response.Result.SessionToken;

                this.Permissions = response.Result.Permissions;

                return response;
            }
        }

        public async Task<ApiResponse<EmptyResponse>> SessionClose()
        {
            var uri = new Uri($"{FreeboxApi.ApiInfo.ApiUri}{BaseModuleUri}logout/");

            var response = await PostAsync<object, EmptyResponse>(null, uri);

            this.LoggedIn = false;
            this.SessionToken = null;
            this.Permissions = new FreeboxPermissions();

            return response;
        }
    }
}
