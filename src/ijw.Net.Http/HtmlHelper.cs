using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace ijw.Net.Http {
    /// <summary>
    /// Html文本的相关帮助类
    /// </summary>
    public static class HtmlHelper {
        /// <summary>
        /// 使用xpath语法检索一个html字符串, 返回符合条件的字符串集合.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="xpath">xpath表达式</param>
        /// <returns>对节点返回InnerText, 对属性返回属性值</returns>
        public static List<string> SelectTextsByXPath(string html, string xpath) {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            var last = xpath.LastIndexOf("/");
            string attr = null;
            if (last < xpath.Length - 2 && xpath[last + 1] == '@') {
                attr = xpath.Substring(last + 2);
                xpath = xpath.GetSubStringPythonStyle(0, last);
            }

            var htmlNodes = htmlDoc.DocumentNode.SelectNodes(xpath);
            if (htmlNodes == null) {
                return null;
            }

            if (attr != null) {
                return htmlNodes.Select((n) => n.GetAttributeValue(attr, "No such attribute")).ToList();
            }
            else {
                return htmlNodes.Select((n) => n.InnerText).ToList();
            }
        }
    }
}
