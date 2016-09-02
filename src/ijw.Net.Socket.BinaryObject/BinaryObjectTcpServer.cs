//TODO: support netstandard when netcore support binaryserialization
using ijw.Serialization.Binary;

namespace ijw.Net.Socket.BinaryObject {
    /// <summary>
    /// ObjectTCPServer&lt;T&gt; 基于TCP协议, 通过监听指定端口, 实现了一个泛型对象远程接收/通知处理服务器. 
    /// 调用者可通过指定ObjectRecievedHandlerAsync委托, 方便地以异步的方式处理新接收到的对象.
    /// 调用者也可以通过挂接事件处理器, 方便地同步处理接收到的对象.
    /// </summary>
    /// <typeparam name="T">获取对象的类型</typeparam>
    /// <remarks>
    /// 调用StartAsync()方法, 内部启动了两个Task. 一个是负责监听TCP端口. 另一个负责发出通知事件.
    /// 两个Task共同维护了内部的一个线程安全的泛型集合. 
    /// </remarks>
    public class BinaryObjectTcpServer<T> : CachedTcpServer<T> where T : class {
        /// <summary>
        /// 初始化服务器实例
        /// </summary>
        /// <param name="ifSupportUIThreading">是否对UI线程操作提供支持. true 则每次接收到对象将会封送至ObjectHandlerAsync所在线程进而可以更改控件. 反之ObjectHandlerAsync将在后台线程运行, 从而无法更改UI线程中的控件.</param>
        public BinaryObjectTcpServer(bool ifSupportUIThreading = false) : base(ifSupportUIThreading) {
            this.RetrieveItemAndDispose = (networkStream) => {
                return networkStream.RetrieveBinaryObjectAndDispose<T>();
            };
        }
    }
}
