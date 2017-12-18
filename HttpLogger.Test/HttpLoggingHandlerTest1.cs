
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace HttpLogger.Test
{
    [TestClass]
    public class HttpLoggingHandlerTest1
    {
        [TestMethod]
        public void Test_HeaderUserAgentHasMultipleLines()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:40.0) Gecko/20100101 Firefox/40.1");

            Assert.AreEqual(4, Regex.Matches(request.ToString(), @"\buser-agent\b").Count);
        }

        [TestMethod]
        public void Test_HeaderUserAgentHasSingleLine()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:40.0) Gecko/20100101 Firefox/40.1");

            string log = HttpLoggingHandler.GetRequestLogString(request);

            Assert.AreEqual(1, Regex.Matches(log, @"\buser-agent\b").Count);
        }
    }
}
