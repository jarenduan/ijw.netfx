#if !NET35
using ijw.Diagnostic;
using ijw.IO;
using ijw.Text;
using ijw.Contract;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace ijw.Net.Http {
    /// <summary>
    /// 提供访问Http/Web的若干帮助方法
    /// </summary>
    public static partial class HttpHelper {
        /// <summary>
        /// 使用指定的编码从指定url下载字符串,。
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="userAgent">浏览器类型，默认是Firefox</param>
        /// <param name="connectTimeout">连接超时时间, 默认是10秒钟</param>
        /// <param name="readTimeout">读取网络流超时时间, 默认是10秒钟</param>
        /// <returns>下载的字符串</returns>
        public static string DownloadString(string url,
                                           Encoding encoding = null,
                                           string userAgent = BrowserUserAgent.Firefox,
                                           int connectTimeout = 1000 * 10,
                                           int readTimeout = 1000 * 10,
                                           bool ifKeepAlive = true,
                                           string referer = null,
                                           CookieContainer oldCookies = null,
                                           string accept = null,
                                           string accept_encoding = null,
                                           string cache_control = null) {
            return GetWebResponseContent(
                url,
                (stream) => stream.ReadStringAndDispose(encoding ?? Encoding.Unicode),
                userAgent,
                connectTimeout,
                readTimeout,
                ifKeepAlive,
                referer,
                oldCookies,
                accept,
                accept_encoding,
                cache_control).result;
        }
              
        /// <summary>
        /// 使用指定的编码方式从指定的URL下载文本数据, 并按指定的编码方式和文件名存为文本文件.
        /// </summary>
        /// <param name="url">指定的url</param>
        /// <param name="filename">指定的文件名</param>
        /// <param name="readEncoding">读取web内容的编码方式</param>
        /// <param name="writeEncoding">写入文件的编码方式</param>
        /// <param name="userAgent">浏览器类型, 默认是Firefox</param>
        /// <param name="connectTimeout">连接超时时间, 默认是10秒钟</param>
        /// <param name="readTimeout">读取网络流超时时间, 默认是10秒钟</param>
        /// <param name="append">是否是追加模式, true为追加, false为新建或覆盖, 默认是false</param>
        /// <returns>下载的字符串长度</returns>
        public static int DownloadStringToFile(string url,
                                               string filename,
                                               Encoding readEncoding = null,
                                               Encoding writeEncoding = null,
                                               string userAgent = BrowserUserAgent.Firefox,
                                               int connectTimeout = 1000 * 10,
                                               int readTimeout = 1000 * 10,
                                               bool ifKeepAlive = true,
                                               string referer = null,
                                               CookieContainer oldCookies = null,
                                               string accept = null,
                                               string accept_encoding = null,
                                               string cache_control = null,
                                               bool append = false) {
            
            return GetWebResponseContent(
                url,
                (stream) =>stream.WriteToTextFileAndDispose(filename, 
                                                            readEncoding ?? Encoding.Unicode, 
                                                            writeEncoding ?? Encoding.Unicode, 
                                                            append),
                userAgent,
                connectTimeout,
                readTimeout,
                ifKeepAlive,
                referer,
                oldCookies,
                accept,
                accept_encoding,
                cache_control).result;
        }

     
        /// <summary>
        /// 使用指定编码从指定的URL下载全部二进制数据
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="encoding">指定的编码</param>
        /// <param name="userAgent">浏览器类型, 默认是Firefox</param>
        /// <param name="connectTimeout">连接超时时间, 默认是10秒钟</param>
        /// <param name="readTimeout">读取网络流超时时间, 默认是10秒钟</param>
        /// <returns>下载的二进制数据</returns>
        public static byte[] DownloadBytes(string url,
                                           Encoding encoding = null, 
                                           string userAgent = BrowserUserAgent.Firefox, 
                                           int connectTimeout = 1000 * 10, 
                                           int readTimeout = 1000 * 10,
                                           bool ifKeepAlive = true,
                                           string referer = null,
                                           CookieContainer oldCookies = null,
                                           string accept = null,
                                           string accept_encoding = null,
                                           string cache_control = null) {
            return GetWebResponseContent(
                url,
                (stream) => stream.ReadBytesAndDispose(encoding ?? Encoding.Unicode),
                userAgent,
                connectTimeout,
                readTimeout,
                ifKeepAlive, 
                referer, 
                oldCookies, 
                accept, 
                accept_encoding, 
                cache_control).result;
        }

    
        /// <summary>
        /// 使用指定编码方式从指定的URL下载二进制数据, 并按指定的编码方式存入指定文件
        /// </summary>
        /// <param name="url">指定的url</param>
        /// <param name="filename">写入的文件名</param>
        /// <param name="readEncoding">读取url内容使用的编码方式</param>
        /// <param name="writeEncoding">写入文件时使用的编码方式</param>
        /// <param name="userAgent">浏览器类型, 默认是Firefox</param>
        /// <param name="connectTimeout">连接超时时间, 默认是10秒钟</param>
        /// <param name="readTimeout">读取网络流超时时间, 默认是10秒钟</param>
        /// <param name="append">写入文件时是否采用追加模式, true为追加, false为新建或覆盖, 默认是false</param>
        /// <returns>下载数据的长度</returns>
        public static long DownloadBytesToFile(string url, 
                                               string filename, 
                                               Encoding readEncoding = null, 
                                               Encoding writeEncoding = null,
                                               string userAgent = BrowserUserAgent.Firefox,
                                               int connectTimeout = 1000 * 10,
                                               int readTimeout = 1000 * 10,
                                               bool ifKeepAlive = true,
                                               string referer = null,
                                               CookieContainer oldCookies = null,
                                               string accept = null,
                                               string accept_encoding = null,
                                               string cache_control = null,
                                               bool append = false) {
            return GetWebResponseContent(
                url,
                (stream) => stream.WriteToBinaryFileAndDispose(filename, 
                                                                readEncoding ?? Encoding.Unicode, 
                                                                writeEncoding ?? Encoding.Unicode, 
                                                                append),
                userAgent,
                connectTimeout,
                readTimeout,
                ifKeepAlive,
                referer,
                oldCookies,
                accept,
                accept_encoding,
                cache_control).result;
        }

        /// <summary>
        /// 获取WebResponse, 并调用指定的委托处理其中的内容, 返回处理后的结果
        /// </summary>
        /// <typeparam name="T">处理后的内容的类型, 如byte[]、string等.</typeparam>
        /// <param name="url">指定的url</param>
        /// <param name="processContent">内容处理委托</param>
        /// <param name="userAgent">浏览器类型, 默认是Firefox</param>
        /// <param name="connectTimeout">连接超时时间, 默认是10秒钟</param>
        /// <param name="readTimeout">读取Response流超时时间, 默认是10秒钟</param>
        /// <returns>处理后的内容</returns>
        public static (T result, CookieContainer cookies) GetWebResponseContent<T>(
            string url,
            Func<Stream, T> func,
            string userAgent = BrowserUserAgent.Firefox,
            int connectTimeout = 1000 * 10,
            int readTimeout = 1000 * 10,
            bool ifKeepAlive = true,
            string referer = null,
            CookieContainer oldCookies = null,
            string accept = null,
            string accept_encoding = null,
            string cache_control = null)
        {
            url.ShouldBeNotNullArgument();
            func.ShouldBeNotNullArgument();

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.UserAgent = userAgent;
            request.Timeout = connectTimeout;
            request.ReadWriteTimeout = readTimeout;
            request.Method = "GET";
            request.KeepAlive = ifKeepAlive;
            request.Referer = referer ?? url;
            if (accept != null) {
                request.Accept = accept;
            }
            if (accept_encoding != null) {
                //request.SendChunked = true;
                //request.TransferEncoding = accept_encoding;
            }
            if (oldCookies != null) {
                request.CookieContainer = oldCookies;
            }
            HttpWebResponse response = null;
            try {
                response = request.GetResponse() as HttpWebResponse;
                Stream receiveStream = response.GetResponseStream();
                T content = func(receiveStream);
                DebugHelper.WriteLine(content.ToString());
                response.Close();
                CookieContainer cookies = request.CookieContainer;
                return (content, cookies);
            }
            catch {
                throw;
            }
            finally {
                response?.Close();
                request?.Abort();
            }
        }
    }
}
#endif
