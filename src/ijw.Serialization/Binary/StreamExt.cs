using System;
using System.IO;

namespace ijw.Serialization.Binary {
    /// <summary>
    /// 扩展Stream类, 以支持一些简便的读写功能.
    /// </summary>
    public static class StreamExt {
 #region Object read and write
        //TODO: support binary for .net standard
#if NET35 || NET40 || NET45 //for netcore is not support binary formatter now, 2016-06-29

        /// <summary>
        /// 使用指定长度从网络流中取回二进制形式序列化的对象, 
        /// </summary>
        /// <typeparam name="T">取回对象的类型</typeparam>
        /// <param name="stream">含有对象二进制数据的流</param>
        /// <param name="length">可以手动指定取回的对象大小. 如果设置为空, 本方法将先读取流中前4个字节, 转换成Int32型整数, 作为对象的大小</param>
        /// <returns>返回解析到的对象, 如果没有读取到对象, 会返回默认值default(T). 没有解析成功, 会抛出异常.</returns>
        /// <remarks>编码采用UTF8</remarks>
        public static T RetrieveBinaryObjectAndDispose<T>(this Stream stream, int? length = null) {
            using (BinaryReader reader = new BinaryReader(stream)) {
                return reader.RetrieveBinaryObject<T>(length);
            }
        }

        /// <summary>
        /// 以二进制方式向流中写入对象(不大于4GB). 对象会先进行二进制序列化,然后写入流中. 
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="stream">流</param>
        /// <param name="obj">目标对象</param>
        /// <param name="writeLengthHeader">是否写入头. 如果为true, 将首先自动写入4个字节的头, 内容是对象序列化后的长度</param>
        /// <remarks>编码采用UTF8</remarks>
        public static void WriteBinaryObjectAndDispose<T>(this Stream stream, T obj, bool writeLengthHeader = true) {
            using (BinaryWriter writer = new BinaryWriter(stream)) {
                writer.WriteBinaryObject(obj, writeLengthHeader);
            }
        }
#endif
        /// <summary>
        /// 以JSON方式向网络流中写入对象. 对象会先进行JSON序列化,然后写入流中. 
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="stream">流</param>
        /// <param name="obj">目标对象</param>
        /// <param name="writeLengthHeader">是否写入头. 如果为true, 将首先自动写入4个字节的头, 内容是对象序列化后的长度</param>
        public static void WriteObjectInJsonAndDispose<T>(this Stream stream, T obj, bool writeLengthHeader = true) {
            //using (MemoryStream mem = new MemoryStream()) {
            //    int objLen = SerializationHelper.SerializeObjectToBinaryStream(obj, mem);
            //    if (writeLengthHeader) {
            //        byte[] objectLenBytes = BitConverter.GetBytes(objLen);
            //        stream.Write(objectLenBytes, 0, objectLenBytes.Length); //write length as header
            //    }
            //    //copy method need to be tested
            //    mem.CopyTo(stream); //write object serialization
            //    stream.Flush();
            //}
            throw new NotImplementedException();
        }
#endregion
    }
}
