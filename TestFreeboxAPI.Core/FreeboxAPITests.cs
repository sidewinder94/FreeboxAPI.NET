using Microsoft.VisualStudio.TestTools.UnitTesting;
using Freebox;
using System.Threading;
using System.Threading.Tasks;
using Freebox.Data.Modules.Login.Requests;
using Freebox.Data.Modules.Login;
using Microsoft.Extensions.Configuration;
using Freebox.Data;
using Freebox.Data.Modules.RRD.Requests;
using System;
using Freebox.Data.Modules.RRD.Responses;
using System.Linq;
using System.Collections.Generic;
using Freebox.Data.Modules;

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
                deviceName: "Unit Test Runner",
                this.Configuration.GetSection("app_token").Value);
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

            var r = api.Login.SessionOpen(appToken).Result;

            Assert.IsTrue(r.Success);
            Assert.IsTrue(api.Login.LoggedIn);
        }

        [TestMethod()]
        public void TestLogout()
        {
            var ctSource = new CancellationTokenSource();
            ctSource.CancelAfter(5000);

            var api = FreeboxAPI.GetFreeboxApiInstance(appInfo, ctSource.Token).Result;

            var r = api.Login.SessionOpen().Result;

            Assert.IsTrue(r.Success);

            var lo = api.Login.SessionClose().Result;

            Assert.IsTrue(lo.Success);
            Assert.IsTrue(string.IsNullOrWhiteSpace(api.Login.SessionToken));
            Assert.IsFalse(api.Login.LoggedIn);
            Assert.IsTrue(lo.Success);
        }

        [TestMethod()]
        public void TestRrd()
        {
            var ctSource = new CancellationTokenSource();
            ctSource.CancelAfter(5000);

            var api = FreeboxAPI.GetFreeboxApiInstance(appInfo, ctSource.Token).Result;

            var session = api.Login.SessionOpen().Result;

            Assert.IsTrue(session.Success);

            var rrd = api.RRD.GetRrd(new Fetch<Net>
            {
                DateStart = DateTime.Now.AddMinutes(-1),
                DateEnd = DateTime.Now,
                Fields = RrdFields.NetRateDown | RrdFields.NetRateUp | RrdFields.NetBwDown | RrdFields.NetBwUp
            }).Result ;

            Assert.IsTrue(rrd.Success);
            Assert.IsTrue(rrd.Result.Data.Any(d => d.RateDown > 0));
            Assert.IsTrue(api.Login.SessionClose().Result.Success);

        }

        [TestMethod()]
        public void TestContinuousRrd()
        {
            var ctSource = new CancellationTokenSource();

            var api = FreeboxAPI.GetFreeboxApiInstance(appInfo, ctSource.Token).Result;

            var listRrd = new List<ApiResponse<RrdResponse<Net>>>();

            ctSource.CancelAfter(120000);
            Task.Run(async () =>
            {
                await foreach (var rrd in api.RRD.StreamRrd<Net>(RrdFields.NetRateDown | RrdFields.NetRateUp, ctSource.Token))
                {
                    listRrd.Add(rrd);
                }
            }).Wait();
            

            Assert.IsTrue(listRrd.Count > 1);
        }

        [TestMethod()]
        public void TestAutoLogin()
        {
            var ctSource = new CancellationTokenSource();
            ctSource.CancelAfter(5000);

            var api = FreeboxAPI.GetFreeboxApiInstance(appInfo, ctSource.Token).Result;

            // The session close method should clear the session token and indicate we are logged out, the test will evolve when it's the case
            var lo = api.RRD.GetRrd(new Fetch<Net>()).Result;

            Assert.IsFalse(string.IsNullOrWhiteSpace(api.Login.SessionToken));
            Assert.IsTrue(api.Login.LoggedIn);
            Assert.IsTrue(lo.Success);
        }
    }
}
