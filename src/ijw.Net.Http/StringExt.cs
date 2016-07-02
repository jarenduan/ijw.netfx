using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace ijw.Net.Http {
    /// <summary>
    /// Html字符串的扩展方法
    /// </summary>
    public static class StringExt {
        /// <summary>
        /// 使用xpath语法检索符合条件的节点集合. 注意, 属性值需再针对节点操作.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static IList<HtmlNode> SelectNodesByXPath(this string html, string xpath) {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            return htmlDoc.DocumentNode.SelectNodes(xpath);
        }

        /// <summary>
        /// 使用xpath语法检索条件的节点或者属性字符串集合.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="xpath">xpath表达式</param>
        /// <returns>对节点返回InnerText, 对属性返回属性值</returns>
        public static List<string> SelectTextsByXPath(this string html, string xpath) {
            return HtmlHelper.SelectTextsByXPath(html, xpath);
        }

        public static bool IsIPv4Address(this string ip) {
            string[] parts = ip.Split('.');
            if (parts.Length != 4) {
                return false;
            }
            for (int i = 0; i < parts.Length; i++) {
                int j;
                if (!int.TryParse(parts[i], out j)) {
                    return false;
                }
                if (i == 0 && (j <= 0 || j > 255)) {
                    return false;
                }
                if (i != 0 && (j < 0 || j > 255)) {
                    return false;
                }
            }

            return true;
        }
    }
}
