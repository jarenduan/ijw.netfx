#if NET35 || NET40 || NET45 //for netcore is not support binary formatter now, 2016-06-29
using ijw.Diagnostic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;


namespace ijw.Serialization.Binary
{
    public class BinarySerializationHelper
    {
        /// <summary>
        /// 把对象序列化成字节数组
        /// </summary>
        /// <param name="objToSave"></param>
        /// <returns>序列化后的数组</returns>
        public static byte[] Serialize(object objToSave) {
            using (MemoryStream stream = new MemoryStream()) {
                Serialize(objToSave, stream);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// 把对象序列化到二进制流当中
        /// </summary>
        /// <param name="objToSave">欲保存的对象(大小&lt;=4GB)</param>
        /// <param name="stream">写入的流</param>
        /// <returns>流的长度</returns>
        internal static int Serialize(object objToSave, Stream stream) {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, objToSave);
            DebugHelper.WriteLine("Object serialized in binary successfully: " + stream.Length);
            return (int)stream.Length;
        }
        
        /// <summary>
        /// 把二进制数组反序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="bytes">存储对象的字节数组</param>
        /// <returns>反序列化后的对象</returns>
        public static T Deserialize<T>(Byte[] bytes) {
            using (MemoryStream mem = new MemoryStream(bytes)) {
                return Deserialize<T>(mem);
            }
        }

        /// <summary>
        /// 从二进制流中反序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="stream">二进制流</param>
        /// <returns>反序列化后的对象</returns>
        internal static T Deserialize<T>(Stream stream) {
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