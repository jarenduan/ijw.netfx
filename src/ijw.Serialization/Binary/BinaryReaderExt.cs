﻿//#if NET35 || NET40 || NET45 //for netcore is not support binary formatter now, 2016-06-29
//20170614 update to netstandard2.0
using ijw.Diagnostic;
using System;
using System.IO;

namespace ijw.Serialization.Binary {
    /// <summary>
    /// BinaryReader的扩展方法
    /// </summary>
    public static class BinaryReaderExt {
        /// <summary>
        /// 使用指定长度从流中取回二进制形式序列化的对象, 
        /// </summary>
        /// <typeparam name="T">取回对象的类型</typeparam>
        /// <param name="reader"></param>
        /// <param name="length">可以手动指定取回的对象大小. 如果设置为空, 本方法将先读取流中前4个字节, 转换成int32型整数, 作为对象的大小</param>
        /// <returns>返回解析到的对象, 如果没有读取到对象, 会返回默认值default(T). 没有解析成功, 会抛出异常.</returns>
        public static T RetrieveBinaryObject<T>(this BinaryReader reader, int? length = null) {
            if (length == null) {
                DebugHelper.Write("Read 4 bytes header...");
                byte[] objLenBytes = reader.ReadBytes(4);
                length = BitConverter.ToInt32(objLenBytes, 0);
            }
            if (length == 0) {
                DebugHelper.WriteLine("No object (length: 0).");
                return default(T);
            }

            DebugHelper.WriteLine(string.Format("Try reading object (length: {0})...", length));
            byte[] objBytes = reader.ReadBytes(length.Value);
            var result = BinarySerializationHelper.Deserialize<T>(objBytes);
            DebugHelper.WriteLine("Object retrieved.");
            return result;
        }
    }
}
//#endif
