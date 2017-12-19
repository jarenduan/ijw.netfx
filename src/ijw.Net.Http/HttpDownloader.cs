#if !NET35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using ijw.IO;
using System.IO;

namespace ijw.Net.Http {
    public class HttpDownloader {
        protected string _url;
        protected Encoding _contentEncoding = Encoding.UTF8;
        protected int _connectTimeout = 1000 * 10;
        protected int _readTimeout = 1000 * 10;
        protected string _userAgent = BrowserUserAgent.Firefox;
        protected bool _ifKeepAlive = true;
        protected string _referer;
        protected CookieContainer _cookies;
        protected string _acceptEncoding;
        protected string _accept;
        protected string _cacheControl;

        public HttpDownloader(string url) {
            this._url = url;
        }

        public HttpDownloader UserAgent(string userAgent) {
            this._userAgent = userAgent;
            return this;
        }

        public HttpDownloader ContentEncoding(Encoding encoding) {
            this._contentEncoding = encoding;
            return this;
        }

        public HttpDownloader ConnectTimeout(int connTimeout) {
            this._connectTimeout = connTimeout;
            return this;
        }

        public HttpDownloader ReadTimeout(int readTimeout) {
            this._readTimeout = readTimeout;
            return this;
        }

        public HttpDownloader IfKeepAlive(bool ifKeepAlive) {
            this._ifKeepAlive = ifKeepAlive;
            return this;
        }

        public HttpDownloader Referer(string referer) {
            this._referer = referer;
            return this;
        }

        public HttpDownloader Cookies(CookieContainer cookies) {
            this._cookies = cookies;
            return this;
        }

        public HttpDownloader Accept(string accept) {
            this._accept = accept;
            return this;
        }

        public HttpDownloader AcceptEncoding(string acceptEncoding) {
            this._acceptEncoding = acceptEncoding;
            return this;
        }

        public HttpDownloader CacheControl(string cacheControl) {
            this._cacheControl = cacheControl;
            return this;
        }

        public string DownloadString() {
            return Download((stream) => stream.ReadStringAndDispose(_contentEncoding));
        }

        public byte[] DownloadBytes() {
            return Download((stream) => stream.ReadBytesAndDispose());
        }

        #region Download to file

        public void DownloadToTextFile(string filename, bool isAppend = false) {
            DownloadToTextFile(filename, Encoding.Default, isAppend);
        }

        public void DownloadToTextFile(string filename, Encoding encoding, bool isAppend = false) {
            Download((s) => { s.WriteToTextFileAndDispose(filename, this._contentEncoding, encoding, isAppend); return 0; });
        }

        public void DownloadToBinaryFile(string filename, bool isAppend = false) {
            DownloadToTextFile(filename, Encoding.Default, isAppend);
        }

        public void DownloadToBinaryFile(string filename, Encoding encoding, bool isAppend = false) {
            Download((s) => { s.WriteToBinaryFileAndDispose(filename, this._contentEncoding, encoding, isAppend); return 0; });
        }
        #endregion

        public T Download<T>(Func<Stream, T> downFunc) {
            var r = HttpHelper.GetWebResponseContent(
                url: this._url,
                func: downFunc,
                userAgent: this._userAgent,
                connectTimeout: this._connectTimeout,
                ifKeepAlive: this._ifKeepAlive,
                referer: _referer ?? _url,
                accept: this._accept,
                accept_encoding: this._acceptEncoding,
                cache_control: this._cacheControl,
                oldCookies: this._cookies
                );
            this._cookies = r.cookies;
            return r.result;
        }
    }
}
#endif