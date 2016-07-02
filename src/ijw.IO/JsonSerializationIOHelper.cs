using System.IO;
using ijw.Serialization.Json;

namespace ijw.IO {
    /// <summary>
    /// 提供若干对象序列化Helper方法
    /// </summary>
    public class JsonSerializationIOHelper {
        /// <summary>
        /// 把 json 文本文件反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="filepath">全路径文件名</param>
        /// <returns>反序列化后的对象</returns>
        public static T LoadJsonObjectFromFile<T>(string filepath) {
            using (StreamReader reader = StreamReaderHelper.NewStreamReader(filepath)) { 
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
                //DebugHelper.WriteLine("into text file: " + filepath);
                return jstring.Length;
            }
        } 
    }
}