#if NET35 || NET40 || NET45 //for netcore is not support binary formatter now, 2016-06-29
using ijw.Diagnostic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ijw.IO
{
    public class BinarySerializationIOHelper
    {
        /// <summary>
        /// 把对象序列化到二进制文件中
        /// </summary>
        /// <param name="objToSave">欲保存的对象</param>
        /// <param name="filename">包含路径的文件名</param>
        public static void Serialize(object objToSave, string filename) {
            using (FileStream fs = new FileStream(filename, FileMode.Create)) {
                BinarySerializationIOHelper.Serialize(objToSave, fs);
                DebugHelper.WriteLine("into binary file: " + filename);
            }
        }

        /// <summary>
        /// 把对象序列化到二进制流当中
        /// </summary>
        /// <param name="objToSave">欲保存的对象(大小&lt;=4GB)</param>
        /// <param name="stream">写入的流</param>
        /// <returns>流的长度</returns>
        public static int Serialize(object objToSave, Stream stream) {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, objToSave);
            DebugHelper.WriteLine("Object serialized in binary successfully: " + stream.Length);
            return (int)stream.Length;
        }

        /// <summary>
        /// 把二进制文件反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="filename">全路径文件名</param>
        /// <returns>反序列化后的对象</returns>
        public static T Deserialize<T>(string filename) {
            using (FileStream fs = File.Open(filename, FileMode.Open)) {
                var obj = BinarySerializationIOHelper.Deserialize<T>(fs);
                DebugHelper.WriteLine("from binary file: " + filename);
                return obj;
            }
        }

        /// <summary>
        /// 从二进制流中反序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="stream">二进制流</param>
        /// <returns>反序列化后的对象</returns>
        public static T Deserialize<T>(Stream stream) {
            BinaryFormatter formatter = new BinaryFormatter();
            DebugHelper.WriteLine("Object deserializing: " + stream.Length.ToString());
            try {
                T obj = (T)formatter.Deserialize(stream);
                DebugHelper.WriteLine("Object deserialized successfully: " + obj.ToString());
                return obj;
            }
            catch (Exception ex) {
                DebugHelper.WriteLine(ex.Message);
            }
            return default(T);
        }
    }
}
#endif