using ijw.Net.Html;
namespace ijw.Net.Http.Utils
{
    public class NetHelper
    {
        /// <summary>
        /// 使用ip.qq.com获取本机外网IP
        /// </summary>
        /// <returns></returns>
        public static string GetExternalIP() {
            string iphtml = HttpHelper.DownloadString("http://ip.qq.com/");
            string ip = iphtml.SelectTextsByXPath("//*[@id=\"search_show\"]/span")[0];
            return ip;
        }
    }
}
