using System;
using ijw.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ijw.Net.Http.Test {
    [TestClass]
    public class HttpHelperTest {
        [TestMethod]
        public void GetWebResponseContentTest() {
            var _1st = HttpHelper.GetWebResponseContent(url: "http://www.141jav.com"
                                        , func: (s) => s.ReadStringAndDispose(System.Text.Encoding.UTF8)
                                        , accept: "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"
                                        , accept_encoding: "gzip, deflate, sdch"
                                        , userAgent: "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.152 Safari/537.36"
                                        , cache_control: "max-age=0"
                                        );

            var _2nd = HttpHelper.GetWebResponseContent(
                                        url: "http://www.141jav.com"
                                        , func: (s) => s.ReadStringAndDispose(System.Text.Encoding.UTF8)
                                        , oldCookies: _1st.cookies
                                        , accept: "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"
                                        , accept_encoding: "gzip, deflate, sdch"
                                        , userAgent: "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.152 Safari/537.36"
                                        , cache_control: "max-age=0"
                                        );
        }
    }
}