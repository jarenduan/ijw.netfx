#if !NET35
using ijw.Net.Html;
using ijw.Net.Http;
using ijw.Text;

namespace ijw.Net.Utils
{
    public class NetHelper
    {
        /// <summary>
        /// 使用ip.qq.com获取本机外网IP
        /// </summary>
        /// <returns></returns>
        public static string GetExternalIP() {
            string iphtml = HttpHelper.DownloadString("http://ip.qq.com/", EncodingHelper.GB2312);
            string ip = iphtml.SelectTextsByXPath("//*[@id=\"search_show\"]/span")[0];
            return ip;
        }
    }
}
#endif
