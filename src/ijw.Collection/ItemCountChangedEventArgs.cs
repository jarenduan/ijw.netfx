using System;
namespace ijw.Collection {
    /// <summary>
    /// 集合元素数量变化事件参数
    /// </summary>
    public class ItemCountChangedEventArgs : EventArgs {
        /// <summary>
        /// 集合元素数量
        /// </summary>
        public int ItemCount { get; set; }
    }
}
