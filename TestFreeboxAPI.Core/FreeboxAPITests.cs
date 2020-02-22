using Microsoft.VisualStudio.TestTools.UnitTesting;
using Freebox;
using System.Threading;
using System.Threading.Tasks;
using Freebox.Data.Modules.Login.Requests;
using Freebox.Data.Modules.Login;
using Microsoft.Extensions.Configuration;

namespace FreeboxApi.Tests
{

    [TestClass()]
    public class FreeboxAPITests
    {
        private IConfigurationRoot Configuration { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<FreeboxAPITests>();
            this.Configuration = builder.Build();
        }

        [TestMethod()]
        public void GetFreeboxApiInstanceTest()
        {
            var ctSource = new CancellationTokenSource();
            ctSource.CancelAfter(5000);

            var api = FreeboxAPI.GetFreeboxApiInstance("Unit Tests FreeboxAPI.NET", ctSource.Token).Result;
            
            Assert.IsNotNull(api.ApiInfo.BoxModelName);
        }

        [TestMethod()]
        public void TestAuthorize()
        {
            var ctSource = new CancellationTokenSource();
            ctSource.CancelAfter(5000);

            var api = FreeboxAPI.GetFreeboxApiInstance("Unit Tests FreeboxAPI.NET", ctSource.Token).Result;

            var authorizeResult = api.Login.Authorize(new AuthorizeCreationRequest()
            {
                AppName = "Unit Tests FreeboxAPI.NET",
                AppVersion = "0.0.7",
                DeviceName = "Unit Test Runner"
            }).Result;

            var authorizeTrack = api.Login.TrackAuthorization(authorizeResult.Result).Result;

            var ctAuth = new CancellationTokenSource(60000);

            while (authorizeTrack.Result.Status == AuthorizeStatus.Pending && !ctAuth.Token.IsCancellationRequested)
            {
                authorizeTrack = api.Login.TrackAuthorization(authorizeResult.Result).Result;

                Task.Delay(200).Wait();
            }

            Assert.IsTrue(authorizeTrack.Result.Status == AuthorizeStatus.Granted
                || authorizeTrack.Result.Status == AuthorizeStatus.Denied
                || authorizeTrack.Result.Status == AuthorizeStatus.Unknown
                || (authorizeTrack.Result.Status == AuthorizeStatus.Pending && ctAuth.IsCancellationRequested));

        }

        [TestMethod()]
        public void TestLogin()
        {
            var ctSource = new CancellationTokenSource();
            ctSource.CancelAfter(5000);

            var api = FreeboxAPI.GetFreeboxApiInstance("Unit Tests FreeboxAPI.NET", ctSource.Token).Result;

            var appToken = this.Configuration.GetSection("app_token").Value;

            var r = api.Login.SessionStart(appToken).Result;

            Assert.IsTrue(r.Success);
        }

    }
}
