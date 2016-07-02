using System;
using ijw.Diagnostic;

namespace ijw {
    public static class EventHandlerExt {
        /// <summary>
        /// 如果有挂接的事件处理器, 就激活事件, 处理完毕后返回一个值.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="sender">sender</param>
        /// <param name="eventArgs">事件参数</param>
        /// <returns>激活了事件, 返回真; 反之返回假</returns>
        public static bool InvokeIfNotNull(this EventHandler handler, object sender, EventArgs eventArgs) {
            //DebugHelper.WriteLine("Try to Invoke ?? event.");
            //TODO: add reflection support for debughelper with handler name
            if (handler != null) {
                handler.Invoke(sender, eventArgs);
                return true;
            }
            else {
                //DebugHelper.WriteLine("Null handler, none invoked.");
                return false;
            }
        }

        /// <summary>
        /// 如果有挂接的事件处理器, 就激活事件, 处理完毕后返回一个值.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        /// <param name="sender">sender</param>
        /// <param name="e">事件参数</param>
        /// <returns>激活了事件, 返回真; 反之返回假</returns>
        public static bool InvokeIfNotNull<T>(this EventHandler<T> handler, object sender, T e)
#if NET35 || NET40
            where T : EventArgs
#endif
        {
            //TODO: add reflection support for debughelper with handler name
            if (handler != null)
            {
                handler.Invoke(sender, e);
                return true;
            }
            else
            {
                DebugHelper.WriteLine("Null handler, none invoked.");
                return false;
            }
        }
    }
}
