#if !NET35 && !NET40 //TODO: support task in net35 and net40
using ijw.Diagnostic;
using ijw.IO;
using ijw.Text;
using System;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace ijw.Net.Http {
    /// <summary>
    /// 提供访问Http/Web的若干帮助方法
    /// </summary>
    public static partial class HttpHelper {
        /// <summary>
        /// 从指定url下载字符串, 使用GB2312编码。
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="userAgent">浏览器类型，默认是Firefox</param>
        /// <param name="connectTimeout">连接超时时间, 默认是10秒钟</param>
        /// <param name="readTimeout">读取网络流超时时间, 默认是10秒钟</param>
        /// <returns>下载的字符串</returns>
        public static async Task<string> DownloadStringAsync(string url, string userAgent = BrowserUserAgent.Firefox, int connectTimeout = 1000 * 10, int readTimeout = 1000 * 10) {
            return await DownloadStringAsync(url, EncodingHelper.GB2312, userAgent, connectTimeout, readTimeout);
        }
        /// <summary>
        /// 使用指定的编码从指定url下载字符串, 。
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="userAgent">浏览器类型，默认是Firefox</param>
        /// <param name="connectTimeout">连接超时时间, 默认是10秒钟</param>
        /// <param name="readTimeout">读取网络流超时时间, 默认是10秒钟</param>
        /// <returns>下载的字符串</returns>
        public static async Task<string> DownloadStringAsync(string url, Encoding encoding, string userAgent = BrowserUserAgent.Firefox, int connectTimeout = 1000 * 10, int readTimeout = 1000 * 10) {
            return await GetWebResponseContentAsync(
                url,
                (stream) => stream.ReadStringAndDispose(encoding),
                userAgent,
                connectTimeout,
                readTimeout);
        }

        /// <summary>
        /// 使用GB2312编码方式从指定的URL下载文本数据, 并按utf-8编码方式存为指定文件
        /// </summary>
        /// <param name="url">指定的url</param>
        /// <param name="filename">指定的文件名</param>
        /// <param name="userAgent">浏览器类型, 默认是Firefox</param>
        /// <param name="connectTimeout">连接超时时间, 默认是10秒钟</param>
        /// <param name="readTimeout">读取网络流超时时间, 默认是10秒钟</param>
        /// <param name="append">是否是追加模式, true为追加, false为新建或覆盖, 默认是false</param>
        /// <returns>下载的字符串长度</returns>
        public static async Task<long> DownloadStringToFileAsync(string url, string filename, string userAgent = BrowserUserAgent.Firefox, int connectTimeout = 1000 * 10, int readTimeout = 1000 * 10, bool append = false) {
            return await DownloadStringToFileAsync(url, filename, EncodingHelper.GB2312, Encoding.UTF8, userAgent, connectTimeout, readTimeout, append);
        }
        /// <summary>
        /// 使用指定的编码方式从指定的URL下载文本数据, 并按指定的编码方式和文件名存为文本文件
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
        public static async Task<long> DownloadStringToFileAsync(string url, string filename, Encoding readEncoding, Encoding writeEncoding, string userAgent = BrowserUserAgent.Firefox, int connectTimeout = 1000 * 10, int readTimeout = 1000 * 10, bool append = false) {
            return await GetWebResponseContentAsync(
                url,
                (stream) => {
                    stream.WriteToTextFileAndDispose(filename, readEncoding, writeEncoding, append);
                    return 0;
                },
                userAgent,
                connectTimeout,
                readTimeout);
        }

        /// <summary>
        /// 使用UTF-8编码从指定的URL下载全部二进制数据
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="userAgent">浏览器类型, 默认是Firefox</param>
        /// <param name="connectTimeout">连接超时时间, 默认是10秒钟</param>
        /// <param name="readTimeout">读取网络流超时时间, 默认是10秒钟</param>
        /// <returns>下载的二进制数据</returns>
        public static async Task<byte[]> DownloadBytesAsync(string url, string userAgent = BrowserUserAgent.Firefox, int connectTimeout = 1000 * 10, int readTimeout = 1000 * 10) {
            return await DownloadBytesAsync(url, Encoding.UTF8, userAgent, connectTimeout, readTimeout);
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
        public static async Task<byte[]> DownloadBytesAsync(string url, Encoding encoding, string userAgent = BrowserUserAgent.Firefox, int connectTimeout = 1000 * 10, int readTimeout = 1000 * 10) {
            return await GetWebResponseContentAsync(
                url,
                (stream) => stream.ReadBytesAndDispose(),
                userAgent,
                connectTimeout,
                readTimeout);
        }

        /// <summary>
        /// 使用Unicode编码从指定的URL下载二进制数据, 并使用Unicode编码存入指定文件
        /// </summary>
        /// <param name="url">指定的url</param>
        /// <param name="filename">指定的文件名</param>
        /// <param name="userAgent">浏览器类型, 默认是Firefox</param>
        /// <param name="connectTimeout">连接超时时间, 默认是10秒钟</param>
        /// <param name="readTimeout">读取网络流超时时间, 默认是10秒钟</param>
        /// <param name="append">是否是追加模式, true为追加, false为新建或覆盖, 默认是false</param>
        /// <returns>下载数据的长度</returns>
        public static async Task<long> DownloadBytesToFileAsync(string url, string filename, string userAgent = BrowserUserAgent.Firefox, int connectTimeout = 1000 * 10, int readTimeout = 1000 * 10, bool append = false) {
            return await DownloadBytesToFileAsync(url, filename, Encoding.Unicode, Encoding.Unicode, userAgent, connectTimeout, readTimeout, append);
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
        public static async Task<long> DownloadBytesToFileAsync(string url, string filename, Encoding readEncoding, Encoding writeEncoding, string userAgent = BrowserUserAgent.Firefox, int connectTimeout = 1000 * 10, int readTimeout = 1000 * 10, bool append = false) {
            return await GetWebResponseContentAsync(
                url,
                (stream) => stream.WriteToBinaryFileAndDispose(filename, readEncoding, writeEncoding, append),
                userAgent,
                connectTimeout,
                readTimeout);
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
        public static async Task<T> GetWebResponseContentAsync<T>(string url, Func<Stream, T> processContent, string userAgent = BrowserUserAgent.Firefox, int connectTimeout = 1000 * 10, int readTimeout = 1000 * 10) {
            var uri = new Uri(url);
            using (HttpClient client = new HttpClient()) {
                if (!client.DefaultRequestHeaders.UserAgent.TryParseAdd(userAgent)) {
                    throw new InvalidUserAgentException(userAgent);
                }
                client.Timeout = TimeSpan.FromMilliseconds(connectTimeout);
                //client.ReadWriteTimeout = readTimeout;
                Stream receiveStream = await client.GetStreamAsync(uri);
                T content = processContent(receiveStream);
                DebugHelper.WriteLine(content.ToString());
                return content;
            }
        }
    }
}
#endif
