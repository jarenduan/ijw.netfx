//TODO: support netstandard when netcore support binaryserialization
using ijw.Collection;
using ijw.Serialization.Binary;

namespace ijw.Net.Socket.BinaryObject {
    /// <summary>
    /// 实现一个带有缓存的对象二进制序列化TCP发送客户端.
    /// </summary>
    /// <typeparam name="T">发送对象的类型</typeparam>
    public class BinaryObjectTcpClient<T> : CachedTcpSendingServer<T> {
        /// <summary>
        /// 构造一个对象发送客户端
        /// </summary>
        /// <param name="getStratrgy">获取对象的策略</param>
        /// <param name="hostName">主机IP, 默认 127.0.0.1</param>
        /// <param name="portNum">端口号, 默认15210(obj三个字母的序号)</param>
        /// <param name="logOn">是否开启日志</param>
        public BinaryObjectTcpClient(string hostName = "127.0.0.1", int portNum = 15210, FetchStrategies getStratrgy = FetchStrategies.First) :
            base(hostName, portNum, getStratrgy) {
            this.WriteItemAndDisposeAction = (networkStream, item) => {
                networkStream.WriteBinaryObjectAndDispose(item);
            };
        }
    }
}
