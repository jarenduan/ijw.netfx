#if !NET35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ijw.Net.Http {
    /// <summary>
    /// Web资源浏览器
    /// </summary>
    public class WebExplorer {
        /// <summary>
        /// 当前网址
        /// </summary>
        public Uri CurrentUri { get; set; }

        /// <summary>
        /// 当前网页内容
        /// </summary>
        public string CurrentContent { get; set; }

        /// <summary>
        /// Cookies
        /// </summary>
        public CookieContainer Cookies { get; set; }

        /// <summary>
        /// Web内容编码，默认UTF8
        /// </summary>
        public Encoding ContentEncoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// 写入的编码方式
        /// </summary>
        public Encoding WriteEncoding { get; set; } = Encoding.Default;

        /// <summary>
        /// 连接超时，默认10s
        /// </summary>
        public int ConnectTimeout = 1000 * 10;

        /// <summary>
        /// 读取超时，默认10s
        /// </summary>
        public int ReadTimeout = 1000 * 10;

        /// <summary>
        /// UserAgent，默认FireFox
        /// </summary>
        public string UserAgent = BrowserUserAgent.Firefox;

        /// <summary>
        /// Keep-Alive
        /// </summary>
        public bool IfKeepAlive = true;

        /// <summary>
        /// Referer
        /// </summary>
        public string Referer;

        /// <summary>
        /// AcceptEncoding
        /// </summary>
        public string AcceptEncoding;

        /// <summary>
        /// Accept
        /// </summary>
        public string Accept;

        /// <summary>
        /// CacheControl
        /// </summary>
        public string CacheControl;

        /// <summary>
        /// 导航到指定网址
        /// </summary>
        /// <param name="Uri">网址字符串</param>
        public void NavigateTo(string Uri) {
            Uri u = new Uri(Uri);
            NavigateTo(u);
        }

        /// <summary>
        /// 导航到指定网址
        /// </summary>
        /// <param name="uri">网址URI</param>
        public void NavigateTo(Uri uri) {
            this.Referer = this.CurrentUri.ToString();
            this.CurrentUri = uri;
        }

        public void Post(string ) {
            this.Referer = this.CurrentUri.ToString();
            this.CurrentUri = Uri;
        }

        public void Get(string uri) {

        }

        public string GetString() {
            return HttpHelper.DownloadString(
                this.CurrentUri.AbsolutePath, 
                this.ContentEncoding, 
                this.UserAgent, 
                this.ConnectTimeout, 
                this.ReadTimeout, 
                this.IfKeepAlive, 
                this.Referer, 
                this.Cookies, 
                this.Accept, 
                this.AcceptEncoding, 
                this.CacheControl);
        }

        public void GetStringToFile(string filename) {
            HttpHelper.DownloadStringToFile(this.CurrentUri.AbsolutePath,
                filename,
                this.ContentEncoding,
                this.WriteEncoding,
                this.UserAgent,
                this.ConnectTimeout,
                this.ReadTimeout,
                this.IfKeepAlive,
                this.Referer,
                this.Cookies,
                this.Accept,
                this.AcceptEncoding,
                this.CacheControl,
                false);
        }

        public byte[] GetBytes() {
            return HttpHelper.DownloadBytes(this.CurrentUri.AbsolutePath,
                this.ContentEncoding,
                this.UserAgent,
                this.ConnectTimeout,
                this.ReadTimeout,
                this.IfKeepAlive,
                this.Referer,
                this.Cookies,
                this.Accept,
                this.AcceptEncoding,
                this.CacheControl);
        }

        public void GetBytesToFile(string filename) {
            HttpHelper.DownloadBytesToFile(
                this.CurrentUri.AbsolutePath, 
                filename,
                this.ContentEncoding, 
                this.WriteEncoding,
                this.UserAgent,
                this.ConnectTimeout,
                this.ReadTimeout,
                this.IfKeepAlive,
                this.Referer,
                this.Cookies,
                this.Accept,
                this.AcceptEncoding,
                this.CacheControl, 
                false);
        }
    }
}
#endif