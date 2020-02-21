using Microsoft.VisualStudio.TestTools.UnitTesting;
using Freebox.Certificates;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace FreeboxApi.Tests
{
    [TestClass()]
    public class CertificateHelperTests
    {
        [TestMethod()]
        public void GetFreeboxRootCertificatesTest()
        {
            var result = CertificateHelper.GetFreeboxRootCertificates();

            Assert.IsTrue(result.OfType<X509Certificate2>().Count() == 2);
        }
    }
}