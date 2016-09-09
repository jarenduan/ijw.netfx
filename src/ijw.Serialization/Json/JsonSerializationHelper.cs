using ijw.Diagnostic;
using ijw.IO;
using Newtonsoft.Json;
using System.IO;

namespace ijw.Serialization.Json {
    /// <summary>
    /// 提供若干对象序列化Helper方法
    /// </summary>
    public class JsonSerializationHelper {
        /// <summary>
        /// 把 json 字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="str">json字符串</param>
        /// <returns>反序列化后的对象</returns>
        public static T LoadJsonObject<T>(string str) {
            return JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// 把对象序列化为JSON字符串
        /// </summary>
        /// <param name="objToSave">欲序列化的对象</param>
        public static string SaveObjectToJsonString(object objToSave) {
            var jstring = JsonConvert.SerializeObject(objToSave);
            DebugHelper.WriteLine("Object serialized in json successfully: " + jstring);
            return jstring;
        }

        /// <summary>
        /// 把 json 文本文件反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="filepath">全路径文件名</param>
        /// <returns>反序列化后的对象</returns>
        public static T LoadJsonObjectFromFile<T>(string filepath) {
            using (StreamReader reader = StreamReaderHelper.NewStreamReaderFrom(filepath)) {
                var jstring = reader.ReadToEnd();
                return JsonSerializationHelper.LoadJsonObject<T>(jstring);
            }
        }

        /// <summary>
        /// 把对象序列化到Json文件中
        /// </summary>
        /// <param name="objToSave">欲序列化的对象</param>
        /// <param name="filepath">文件全路径名</param>
        /// <returns></returns>
        public static long SaveObjectToJsonFile(object objToSave, string filepath) {
            using (StreamWriter w = StreamWriterHelper.NewStreamWriterByFilepath(filepath)) {
                string jstring = JsonSerializationHelper.SaveObjectToJsonString(objToSave);
                w.Write(jstring);
                DebugHelper.WriteLine("into text file: " + filepath);
                return jstring.Length;
            }
        }
    }
}