using Microsoft.VisualStudio.TestTools.UnitTesting;
using Freebox;
using System.Threading;
using System.Threading.Tasks;
using Freebox.Data.Modules.Login.Requests;
using Freebox.Data.Modules.Login;
using Microsoft.Extensions.Configuration;
using Freebox.Data;

namespace FreeboxApi.Tests
{

    [TestClass()]
    public class FreeboxAPITests
    {
        private IConfigurationRoot Configuration { get; set; }
        private AppInfo appInfo;

        [TestInitialize]
        public void TestInitialize()
        {
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<FreeboxAPITests>();
            this.Configuration = builder.Build();

            this.appInfo = new AppInfo(
                appId: "Unit Tests FreeboxAPI.NET",
                appName: "Unit Tests FreeboxAPI.NET",
                appVersion: "0.0.7",
                deviceName: "Unit Test Runner");
        }

        [TestMethod()]
        public void GetFreeboxApiInstanceTest()
        {
            var ctSource = new CancellationTokenSource();
            ctSource.CancelAfter(5000);

            var api = FreeboxAPI.GetFreeboxApiInstance(appInfo, ctSource.Token).Result;
            
            Assert.IsNotNull(api.ApiInfo.BoxModelName);
        }

        [TestMethod()]
        public void TestAuthorize()
        {
            var ctSource = new CancellationTokenSource();
            ctSource.CancelAfter(5000);

            var api = FreeboxAPI.GetFreeboxApiInstance(appInfo, ctSource.Token).Result;

            var authorizeResult = api.Login.Authorize().Result;

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

            System.Console.WriteLine(authorizeResult.Result.AppToken);
        }

        [TestMethod()]
        public void TestLogin()
        {
            var ctSource = new CancellationTokenSource();
            ctSource.CancelAfter(5000);

            var api = FreeboxAPI.GetFreeboxApiInstance(appInfo, ctSource.Token).Result;

            var appToken = this.Configuration.GetSection("app_token").Value;

            var r = api.Login.SessionStart(appToken).Result;

            Assert.IsTrue(r.Success);
        }

    }
}
