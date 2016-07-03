using ijw.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ijw.IO {
    /// <summary>
    /// 扩展Stream类, 以支持一些简便的读写功能.
    /// </summary>
    public static class StreamExt {
        #region Read String

        /// <summary>
        /// 使用Unicode编码方式使用StreamReader从流中读取全部的字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ReadString(this Stream stream) {
            return stream.ReadString(Encoding.Unicode);
        }

        /// <summary>
        /// 使用指定的编码方式,调用StreamReader从流中读取全部的字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        public static string ReadString(this Stream stream, Encoding encoding) {
            using (StreamReader reader = new StreamReader(stream, encoding)) {
                return reader.ReadToEnd();
            }
        }

        #endregion

        #region Read Bytes

        /// <summary>
        /// 使用Unicode编码方式, 调用BinaryReader从流中读取指定长度的二进制数据
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="length">读取长度</param>
        /// <returns>读取的二进制数组</returns>
#if NETSTANDARD1_4
        public static byte[] ReadBytes(this Stream stream, int length) {
#else
        public static byte[] ReadBytes(this Stream stream, long length) {
#endif
            return stream.ReadBytes(length, Encoding.Unicode);
        }

        /// <summary>
        /// 使用指定的编码方式, 调用BinaryReader从流中读取指定长度的二进制数据
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="length">读取长度</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>读取的二进制数组</returns>
#if NETSTANDARD1_4
        public static byte[] ReadBytes(this Stream stream, int length, Encoding encoding) {
#else
        public static byte[] ReadBytes(this Stream stream, long length, Encoding encoding) {
#endif
            using (BinaryReader reader = new BinaryReader(stream, encoding)) {
                return reader.ReadBytes(length);
            }
        }

        /// <summary>
        /// 使用Unicode编码方式, 调用BinaryReader从流中读取全部二进制数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>读取的字节数</returns>
        public static byte[] ReadBytes(this Stream stream) {
            return stream.ReadBytes(Encoding.Unicode);
        }

        /// <summary>
        /// 使用BinaryReader用指定的编码方式从流中读取全部的二进制数据.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding">使用的编码方式</param>
        /// <returns>读取的字节数</returns>
        public static byte[] ReadBytes(this Stream stream, Encoding encoding) {
            using (BinaryReader reader = new BinaryReader(stream, encoding)) {
                return reader.ReadBytes().ToArray();
            }
        }

#endregion

#region Write Text File
        /// <summary>
        /// 使用UTF8编码调用ReadStringByStreamReader方法读取流中的全部字符串, 使用UTF8编码覆盖或者追加到指定文件.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="filename">文件名</param>
        /// <param name="append">是否追加. 默认是false</param>
        public static void WriteToTextFile(this Stream stream, string filename, bool append = false) {
            stream.WriteToTextFile(filename, System.Text.Encoding.GetEncoding("utf-8"), System.Text.Encoding.GetEncoding("utf-8"), append);
        }
        /// <summary>
        /// 使用指定的编码方式调用ReadStringByStreamReader方法读取流中的全部字符串, 然后使用指定编码覆盖或者追加到指定文件.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="filename">写入的文件</param>
        /// <param name="readEncoding">读取流用的编码</param>
        /// <param name="writeEncoding">写入文件的编码方式</param>
        /// <param name="append">是否追加. 默认是false</param>
        public static void WriteToTextFile(this Stream stream, string filename, Encoding readEncoding, Encoding writeEncoding, bool append = false) {
            using (StreamWriter writer = StreamWriterHelper.NewStreamWriterByFilepath(filename, writeEncoding, append)) {
                var s = stream.ReadString(readEncoding);
                writer.Write(s);
                writer.Flush();
            }
        }

#endregion

#region Write Binary File
        /// <summary>
        /// 使用Unicode编码调用ReadBytesByBinaryReader方法读取流中的全部二进制数据, 然后使用Unicode编码覆盖或者追加到指定文件.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="filename">写入的文件</param>
        /// <param name="append">是否追加. 默认是false</param>
        public static void WriteToBinaryFile(this Stream stream, string filename, bool append = false) {
            stream.WriteToBinaryFile(filename, Encoding.Unicode, Encoding.Unicode, append);
        }
        /// <summary>
        /// 使用指定的编码方式调用ReadBytesByBinaryReader方法读取流中指定长度的二进制数据, 然后使用指定编码覆盖(或追加)到指定文件.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="filename">写入的文件</param>
        /// <param name="length">读取数据字节数</param>
        /// <param name="readEncoding">读取流时使用的编码方式</param>
        /// <param name="writeEncoding">写入文件时使用的编码方式</param>
        /// <param name="append">是否追加. 默认是false</param>
        /// <returns>读取到的二进制数组</returns>
#if NETSTANDARD1_4
        /// 
        public static byte[] WriteToBinaryFile(this Stream stream, string filename, int length, Encoding readEncoding, Encoding writeEncoding, bool append = false) {
#else
        public static byte[] WriteToBinaryFile(this Stream stream, string filename, long length, Encoding readEncoding, Encoding writeEncoding, bool append = false) {
#endif
            FileMode filemode = append ? FileMode.Append : FileMode.Create;
            FileStream file = new FileStream(filename, filemode);
            using (BinaryWriter writer = new BinaryWriter(file, writeEncoding)) {
                byte[] content = stream.ReadBytes(length, readEncoding);
                writer.Write(content);
                writer.Flush();
                return content;
            }
        }
            /// <summary>
            /// 使用指定的编码方式调用ReadBytesByBinaryReader方法读取流中的全部二进制数据, 然后使用指定编码覆盖或者追加到指定文件.
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="filename">写入的文件</param>
            /// <param name="readEncoding">读取流时使用的编码方式</param>
            /// <param name="writeEncoding">写入文件时使用的编码方式</param>
            /// <param name="append">是否追加. 默认是false</param>
        public static long WriteToBinaryFile(this Stream stream, string filename, Encoding readEncoding, Encoding writeEncoding, bool append = false) {
            FileMode filemode = append ? FileMode.Append : FileMode.Create;
            long length = 0;
            FileStream file = new FileStream(filename, filemode);
            using (BinaryWriter writer = new BinaryWriter(file, writeEncoding)) {
                using (BinaryReader reader = new BinaryReader(stream, readEncoding)) {
                    byte[] buffer = reader.ReadBytes(256);
                    //int i = 0;
                    while (buffer.Length > 0) {
                        length += buffer.Length;
                        writer.Write(buffer);
                        buffer = reader.ReadBytes(256);// read next 256 bytes
                        //Console.Write(i++);
                    }
                }
                writer.Flush();
            }
            return length;
        }
#endregion

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
        public static void WriteObjectInJSON<T>(this Stream stream, T obj, bool writeLengthHeader = true) {
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
