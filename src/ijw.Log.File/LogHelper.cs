//实现采用ijw.dotnet.SimpleLog
//log4net的配置不能自动化，比较麻烦，不符合ijw基本原则。
using System;

namespace ijw.Log.File {
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public class LogHelper : ILogHelper {
        private SimpleFileLog _logger = new SimpleFileLog();

        /// <summary>
        /// 日志文件路径, 默认文件当前目录的~.log。
        /// </summary>
        public string LogFilePath {
            get { return this._logger.LogFilePath; }
            set { this._logger.LogFilePath = value; }
        }

        /// <summary>
        /// 输出错误日志到SimpleLog，前面会标明Error字样
        /// </summary>
        /// <param name="ex"></param>
        public void WriteError(Exception ex) {
            if (ex == null) {
                return;
            }
            this._logger.Log("Error： " + ex.Message);
        }
        /// <summary>
        /// 输出错误日志到SimpleLog，前面会标明Error字样
        /// </summary>
        /// <param name="msg"></param>
        public void WriteError(string msg) {
            if (msg == null) {
                return;
            }
            this._logger.Log("Error： " + msg);
        }

        /// <summary>
        /// 输出信息日志到SimpleLog，前面会标明Info字样
        /// </summary>
        /// <param name="ex"></param>
        public void WriteInfo(Exception ex) {
            if (ex == null) {
                return;
            }
            this._logger.Log("Info: " + ex.Message);
        }
        /// <summary>
        /// 输出信息日志到SimpleLog，前面会标明Info字样
        /// </summary>
        /// <param name="msg"></param>
        public void WriteInfo(string msg) {
            if (msg == null) {
                return;
            }
            this._logger.Log("Info: " + msg);
        }
    }
}
