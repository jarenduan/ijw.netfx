#if NET35 || NET40 || NET45 //for netcore is not support binary formatter now, 2016-06-29
using ijw.Diagnostic;
using System;
using System.IO;

namespace ijw.Serialization.Binary {
    public static class BinaryWriterExt {
        /// <summary>
        /// 对象会先进行二进制序列化, 然后以二进制方式向流中写入(不大于4GB). 
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="writer">流</param>
        /// <param name="obj">目标对象</param>
        /// <param name="writeLengthHeader">是否写入头. 如果为true, 将首先自动写入4个字节的头, 内容是对象序列化后的长度</param>
        /// <remarks>编码采用UTF8</remarks>
        public static void WriteBinaryObject<T>(this BinaryWriter writer, T obj, bool writeLengthHeader = true) {
            using(MemoryStream mem = new MemoryStream()) {
                int len = BinarySerializationHelper.Serialize(obj, mem);
                if(writeLengthHeader) {
                    byte[] objectLenBytes = BitConverter.GetBytes(len);
                    DebugHelper.WriteLine(string.Format("Write Length {0} as 4 Bytes header!", objectLenBytes.Length));
                    writer.Write(objectLenBytes); //write length as header
                }
                DebugHelper.WriteLine("Try to write object.");
                var objBytes = mem.GetBuffer();
                writer.Write(objBytes);
                DebugHelper.WriteLine("Object wrote.");
            }
        }
    }
}
#endif
