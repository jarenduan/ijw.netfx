using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ijw.Text;

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
        public static string ReadStringAndDispose(this Stream stream) {
            return stream.ReadStringAndDispose(Encoding.Unicode);
        }

        /// <summary>
        /// 使用指定的编码方式,调用StreamReader从流中读取全部的字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        public static string ReadStringAndDispose(this Stream stream, Encoding encoding) {
            using (StreamReader reader = new StreamReader(stream, encoding)) {
                return reader.ReadToEnd();
            }
        }

        #endregion

        #region Write String
        public static void WriteStringAndDispose(this Stream stream, string aString, Encoding encoding) {
            using (var writer = new StreamWriter(stream, encoding)) {
                writer.Write(aString);
            }
        }

        public static void WriteStringAndDispose(this Stream stream, string aString) {
            using (var writer = new StreamWriter(stream, Encoding.Unicode)) {
                writer.Write(aString);
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
        public static byte[] ReadBytesAndDispose(this Stream stream, int length) {
#else
        public static byte[] ReadBytesAndDispose(this Stream stream, long length) {
#endif
            return stream.ReadBytesAndDispose(length, Encoding.Unicode);
        }

        /// <summary>
        /// 使用指定的编码方式, 调用BinaryReader从流中读取指定长度的二进制数据
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="length">读取长度</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>读取的二进制数组</returns>
#if NETSTANDARD1_4
        public static byte[] ReadBytesAndDispose(this Stream stream, int length, Encoding encoding) {
#else
        public static byte[] ReadBytesAndDispose(this Stream stream, long length, Encoding encoding) {
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
        public static byte[] ReadBytesAndDispose(this Stream stream) {
            return stream.ReadBytesAndDispose(Encoding.Unicode);
        }

        /// <summary>
        /// 使用BinaryReader用指定的编码方式从流中读取全部的二进制数据.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding">使用的编码方式</param>
        /// <returns>读取的字节数</returns>
        public static byte[] ReadBytesAndDispose(this Stream stream, Encoding encoding) {
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
        public static void WriteToTextFileAndDispose(this Stream stream, string filename, bool append = false) {
            stream.WriteToTextFileAndDispose(filename, System.Text.Encoding.GetEncoding("utf-8"), System.Text.Encoding.GetEncoding("utf-8"), append);
        }
        /// <summary>
        /// 使用指定的编码方式调用ReadStringByStreamReader方法读取流中的全部字符串, 然后使用指定编码覆盖或者追加到指定文件.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="filename">写入的文件</param>
        /// <param name="readEncoding">读取流用的编码</param>
        /// <param name="writeEncoding">写入文件的编码方式</param>
        /// <param name="append">是否追加. 默认是false</param>
        public static void WriteToTextFileAndDispose(this Stream stream, string filename, Encoding readEncoding, Encoding writeEncoding, bool append = false) {
            using (StreamWriter writer = StreamWriterHelper.NewStreamWriterByFilepath(filename, writeEncoding, append)) {
                var s = stream.ReadStringAndDispose(readEncoding);
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
        public static void WriteToBinaryFileAndDispose(this Stream stream, string filename, bool append = false) {
            stream.WriteToBinaryFileAndDispose(filename, Encoding.Unicode, Encoding.Unicode, append);
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
        public static byte[] WriteToBinaryFileAndDispose(this Stream stream, string filename, long length, Encoding readEncoding, Encoding writeEncoding, bool append = false) {
#endif
            FileMode filemode = append ? FileMode.Append : FileMode.Create;
            FileStream file = new FileStream(filename, filemode);
            using (BinaryWriter writer = new BinaryWriter(file, writeEncoding)) {
                byte[] content = stream.ReadBytesAndDispose(length, readEncoding);
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
        public static long WriteToBinaryFileAndDispose(this Stream stream, string filename, Encoding readEncoding, Encoding writeEncoding, bool append = false) {
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
    }
}
