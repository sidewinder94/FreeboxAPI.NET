using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Freebox;
using System.Threading;
using System.Threading.Tasks;
using Freebox.Data.Modules.Login.Requests;
using Freebox.Data.Modules.Login;

namespace FreeboxApi.Tests
{
    [TestClass()]
    public class FreeboxAPITests
    {
        [TestMethod()]
        public void GetFreeboxApiInstanceTest()
        {
            var ctSource = new CancellationTokenSource();
            ctSource.CancelAfter(5000);

            var api = FreeboxAPI.GetFreeboxApiInstance(ctSource.Token).Result;


            Assert.IsNotNull(api.ApiInfo.BoxModelName);
        }

        [TestMethod()]
        public void TestAuthorize()
        {
            var ctSource = new CancellationTokenSource();
            ctSource.CancelAfter(5000);

            var api = FreeboxAPI.GetFreeboxApiInstance(ctSource.Token).Result;

            var authorizeResult = api.Login.Authorize(new AuthorizeCreationRequest()
            {
                AppId = "Unit Tests FreeboxAPI.NET",
                AppName = "Unit Tests FreeboxAPI.NET",
                AppVersion = "0.0.7",
                DeviceName = "Unit Test Runner"
            }).Result;

            var authorizeTrack = api.Login.TrackAuthorization(authorizeResult.Result).Result;

            var ctAuth = new CancellationTokenSource(60000);

            while(authorizeTrack.Result.Status == AuthorizeStatus.Pending && !ctAuth.Token.IsCancellationRequested)
            {
                authorizeTrack = api.Login.TrackAuthorization(authorizeResult.Result).Result;

                Task.Delay(200).Wait();
            }


            Assert.IsTrue(authorizeTrack.Result.Status == AuthorizeStatus.Granted
                || authorizeTrack.Result.Status == AuthorizeStatus.Denied
                || authorizeTrack.Result.Status == AuthorizeStatus.Unknown
                || (authorizeTrack.Result.Status == AuthorizeStatus.Pending && ctAuth.IsCancellationRequested));
        }
    }
}
