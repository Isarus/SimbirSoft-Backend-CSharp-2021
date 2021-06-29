using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Backend_1.Tests
{
    [TestClass]
    public class UrlStatisticsTests
    {
        [TestMethod]
        public void TestUrlVerified()
        {
            string[] urls =
            {
                "https://www.example.com",
                "http://www.example.com",
                "http://www.example.co.uk",
                "http://blog.example.com",
                "http://www.example.com/product",
                "http://www.example.com/products?id=1&page=2",
                "http://www.example.com#up",
                "http://255.255.255.255",
                "http://www.site.com:8008"
            };

            UrlStatistics statistics = new UrlStatistics();

            foreach (var url in urls)
            {
                bool verified = statistics.VerifyURL(url);
                Assert.IsTrue(verified);
            }
        }

        [TestMethod]
        public void TestUrlNotVerified()
        {
            string[] urls =
            {
                "www.example.com",
                "example.com",
                "255.255.255.255",
                "http://invalid.com/perl.cgi?key= | http://web-site.com/cgi-bin/perl.cgi?key1=value1&key2",
            };

            UrlStatistics statistics = new UrlStatistics();

            foreach (var url in urls)
            {
                bool verified = statistics.VerifyURL(url);
                Assert.IsFalse(verified);
            }
        }

        [TestMethod]
        public void TestWordsStatisticsMethod()
        {
            string testPage = "<!DOCTYPE html> " +
                "<html>" +
                    " <head>" +
                        " <title> Lorem ipsum dolor sit amen </title>" +
                        " <style type=\"text/css\">" +
                            " body{font-family: \"Lorem ipsum dolor sit amen\"; }" +
                        " </style>" +
                        " <script type=\"text/javascript\">" +
                            " var a = \"Lorem ipsum dolor sit amen\";" +
                        " </script>" +
                    " </head>" +
                    " <body>" +
                        " <p>lorem ipsum dolor sit amen</p>" +
                        " <i>Lorem ipsum dolor sit amen</i>" +
                        " lorem ipsum dolor sit amen" +
                    " </body>" +
                " </html>";

            UrlStatistics urlStatistics = new UrlStatistics();
            urlStatistics.statisticsMethod = new UrlWordsStatisticsMethod();

            string statistics = urlStatistics.GetStatistics(testPage);
            bool rightCount = statistics.Contains("lorem - 4");

            Assert.IsTrue(rightCount);
        }
    }
}
