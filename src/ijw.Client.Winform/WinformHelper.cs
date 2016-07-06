using ijw.IO;
using ijw.Serialization.Binary;
using System;
using System.IO;
using System.Windows.Forms;

namespace ijw.Winform {
    public class WinformHelper {
        /// <summary>
        /// 透过OpenFileDialog打开文件, 用一个扩展名进行默认过滤.
        /// 将会进行二进制反序列化.
        /// </summary>
        /// <typeparam name="T">文件对象类型</typeparam>
        /// <param name="FilterRemark">打开的文件类型</param>
        /// <param name="FilterExtensionName">扩展名</param>
        /// <returns>文件反序列化的对象</returns>
        public static T GetFileByOpenFileDialog<T>(string FilterRemark = "", string FilterExtensionName = "") {
            OpenFileDialog ofd = new OpenFileDialog();
            setFilter(FilterRemark, FilterExtensionName, ofd);
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK) {
                try {
                    Stream myStream = null;
                    if ((myStream = ofd.OpenFile()) != null) {
                        using (myStream) {
                           return BinarySerializationHelper.Deserialize<T>(myStream);
                        }
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

            return default(T);
        }

        /// <summary>
        /// 通过OpenFileDialog选择一个文件全路径名. 注意, 不保证返回的文件全名有效
        /// </summary>
        /// <param name="FilterRemark">文件类型过滤的说明, 如: 文本文件</param>
        /// <param name="FilterExtensionName">文件扩展名, 如: .txt</param>
        /// <returns>一个文件的全路径名, 如果用户取消选择, 返回string.Empty</returns>
        /// <remarks>
        /// 如果想获得可靠的文件路径信息, 可以调用 SelectFileInfoByOpenFileDialog 方法.
        /// </remarks>
        public static string SelectFileByOpenFileDialog(string FilterRemark, string FilterExtensionName) {
            OpenFileDialog ofd = new OpenFileDialog();
            setFilter(FilterRemark, FilterExtensionName, ofd);
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK) {
                return ofd.FileName;
            }
            else {
                return string.Empty;
            }
        }

        /// <summary>
        /// 通过OpenFileDialog选择一个FileInfo. 这可以保证文件路径名的有效.
        /// </summary>
        /// <param name="FilterRemark">文件类型过滤的说明, 如: 文本文件</param>
        /// <param name="FilterExtensionName">文件扩展名, 如: .txt</param>
        /// <returns>一个文件的全路径名, 如果用户取消选择, 返回string.Empty</returns>
        public static string SelectFileInfoByOpenFileDialog(string FilterRemark, string FilterExtensionName) {
            OpenFileDialog ofd = new OpenFileDialog();
            setFilter(FilterRemark, FilterExtensionName, ofd);
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK) {
                return ofd.FileName;
            }
            else {
                return string.Empty;
            }
        }

        private static void setFilter(string FilterRemark, string FilterExtensionName, FileDialog fd) {
            var extName = FilterExtensionName.TrimStart('.');
            if (FilterExtensionName == null
                || FilterExtensionName.Equals(string.Empty)
                || FilterRemark == null
                || FilterRemark.Equals(string.Empty)) {
                return;
            }
            else {
                fd.Filter = string.Format("{0} (*.{1})|*.{1}|All files (*.*)|*.*", FilterRemark, extName);
                fd.FilterIndex = 1;
            }
        }

        /// <summary>
        /// 透过SaveFileDialog保存文件, 用一个扩展名进行默认过滤
        /// 将会进行二进制序列化, 写入到指定文件中.
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="fileObject">欲保存成文件的对象</param>
        /// <param name="FilterRemark">文件类型过滤的说明, 如: 文本文件</param>
        /// <param name="FilterExtensionName">文件扩展名, 如: .txt</param>
        /// <returns>是否成功完成. 如果用户点击取消也返回false</returns>
        public static bool TrySaveFileBySaveFileDialog<T>(T fileObject, string FilterRemark = "", string FilterExtensionName = "") {
            SaveFileDialog sfd = new SaveFileDialog();
            setFilter(FilterRemark, FilterExtensionName, sfd);
            if (sfd.ShowDialog() == DialogResult.OK) {
                try {
                    BinarySerializationHelper.Serialize(fileObject, sfd.FileName);
                    return true;
                    //Stream myStream = null;
                    //if ((myStream = sfd.OpenFile()) != null) {
                    //    using (myStream) {
                    //        SerializationHelper.SerializeObjectToBinaryStream(fileObject, myStream);
                    //        return true;
                    //    }
                    //}
                }
                catch (Exception ex) {
                    MessageBox.Show("Error: Could not save file to disk. Original error: " + ex.Message);
                }
            }
            return false;
        }
    }
}