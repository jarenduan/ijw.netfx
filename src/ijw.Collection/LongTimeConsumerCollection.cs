using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ijw.Diagnostic;

namespace ijw.Collection {
    /// <summary>
    /// 提供一个支持较长时间消费操作的线程安全集合.
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <remarks>
    ///  消费者可以随时追加元素，也可以随时取出元素，进行消费操作。
    ///  消费后，可以通过Remove方法控制何时移除对象, 满足长时间消费操作的需求.
    ///  如果消费者最终无法完成消费操作, 可以调用Return将对象还回集合.
    /// </remarks>
    public class LongTimeConsumerCollection<T> {
        /// <summary>
        /// 内部元素数量发生变化
        /// </summary>
        public event EventHandler<ItemCountChangedEventArgs> ItemCountChanged;
        /// <summary>
        /// 元素取出策略
        /// </summary>
        public FetchStrategies ItemGettngStrategy { get; set; }

        /// <summary>
        /// 元素数量.
        /// </summary>
        public int Count => this._itemsList.Count;

        /// <summary>
        /// 获取是否存在元素
        /// </summary>
        public bool HasItem => this._itemsList.Count != 0;

        /// <summary>
        /// 获取当前是否存在未被提取消费的元素
        /// </summary>
        public bool IsItemAvailable => this._itemsList.Exists((tuple) => tuple.Item2 == false);

        /// <summary>
        /// 向集合尾部追加一个元素
        /// </summary>
        /// <param name="item"></param>
        public void Append(T item) {
            if(item == null) {
                DebugHelper.WriteLine("(Appending Item) Item is null, so quit appending.");
                return;
            }
            DebugHelper.WriteLine("(Appending Item) Try to get the lock...");
            int count;
            lock(this._syncRoot) {
                DebugHelper.WriteLine("(Appending Item) Got the lock!");
                DebugHelper.Write($"(Appending Item) Item count :{_itemsList.Count}");
                this._itemsList.Add(new MutableTuple<T, bool>() { 
                    Item1 = item, 
                    Item2 = false 
                });
                //this._items.Add(item, false);
                count = _itemsList.Count;
                Debug.WriteLine($" => {count}. Item appended!");
            }
            RaiseCountChangedEvent(count);
        }
        /// <summary>
        /// 移除某个元素
        /// </summary>
        /// <param name="item"></param>
        public void Remove(T item) {
            if(item == null) {
                DebugHelper.WriteLine("(Removing Item) Item is null, so quit removing.");
                return;
            }
            DebugHelper.WriteLine("(Removing Item) Try to get the lock...");
            int index = GetIndexOf(item);
            if(index == -1) {
                DebugHelper.Write("(Removing Item) Not Found the Item.");
                return;
            }
            int count;
            lock (this._syncRoot) {
                DebugHelper.WriteLine("(Removing Item) Got the lock!");
                DebugHelper.Write($"(Removing Item) Item count :{_itemsList.Count}");
                index = GetIndexOf(item);
                if(index == -1) {
                    DebugHelper.Write("(Removing Item) Not Found the Item.");
                    return;
                }
                this._itemsList.RemoveAt(index); //RemoveAt is O(count - index) operation.
                count = _itemsList.Count;
                Debug.WriteLine($" => {count}. Item removed!");
            }
            RaiseCountChangedEvent(count);
        }

        private int GetIndexOf(T item) {
            return this._itemsList.IndexOf(t => t.Item1.Equals(item), this.ItemGettngStrategy);
        }

        private void RaiseCountChangedEvent(int count) {
            var temp = this.ItemCountChanged;
            if(temp != null) {
                this.ItemCountChanged(this, new ItemCountChangedEventArgs() { ItemCount = count });
            }
        }
        /// <summary>
        /// 将指定元素交还给集合
        /// </summary>
        /// <param name="item">欲交还的元素</param>
        public void Return(T item) {
            if(item == null) {
                DebugHelper.WriteLine("(Returning Item Back) Item is null, so quit returning.");
                return;
            }
            int index = GetIndexOf(item);
            if(index == -1) {
                DebugHelper.WriteLine("(Returning Item Back) Item is not in collection, so quit returning.");
                return;
            }
            DebugHelper.WriteLine("(Returning Item Back) Try to get the lock...");
            lock(this._syncRoot) {
                DebugHelper.WriteLine("(Returning Item Back) Got the lock!");
                index = GetIndexOf(item);
                if(index == -1) {
                    DebugHelper.WriteLine("(Returning Item Back) But item returning back is not in collection, so quit returning.");
                    return;
                }
                this._itemsList[index].Item2 = false;
                DebugHelper.Write($"(Returning Item Back) Item returned, count:{_itemsList.Count}");
            }
        }

        /// <summary>
        /// 尝试取出一个元素
        /// </summary>
        /// <param name="item">取出的元素</param>
        /// <returns>是否成功</returns>
        public bool TryBorrow(out T item) {
            return TryGetItem(out item, false);
        }

        /// <summary>
        /// 尝试借出一个可用元素
        /// </summary>
        /// <param name="item">取出的元素</param>
        /// <returns>是否成功</returns>
        public bool TryBorrowAvailable(out T item) {
            return TryGetItem(out item, true);
        }
        private bool TryGetItem(out T item, bool onlyGetNotInConsuming = false) {
            item = default(T);
            if(!this.HasItem) {
                DebugHelper.WriteLine("(Getting Item) Try getting Item, but no items.");
                return false;
            }
            if(onlyGetNotInConsuming && !this.IsItemAvailable) {
                DebugHelper.WriteLine("(Getting Item) Try getting Item, but all items are in consuming.");
                return false;
            }
            DebugHelper.WriteLine("(Getting Item) Try to get the lock...");
            int index = -1;
            lock (this._syncRoot) {
                DebugHelper.WriteLine("(Getting Item) Got the lock!");
                if(!this.HasItem) { 
                    DebugHelper.WriteLine("(Getting Item) But no items :("); 
                    return false; 
                }
                if(onlyGetNotInConsuming && !this.IsItemAvailable) {
                    DebugHelper.WriteLine("(Getting consuming Item) But all in consuming :(");
                    return false;
                }
                if(onlyGetNotInConsuming) {
                    index = this._itemsList.IndexOf(t => t.Item2 == false, ItemGettngStrategy);
                    DebugHelper.WriteLine($"(Getting Non-consuming Item) Non-consuming item count: {_itemsList.Count(i => i.Item2 == false)}.");
                    DebugHelper.WriteLine($"(Getting Non-consuming Item) Got one non-consuming item using {ItemGettngStrategy.ToString()}");
                }
                else {
                    DebugHelper.WriteLine($"(Getting Item) Item count: {_itemsList.Count}.");
                    switch(this.ItemGettngStrategy) {
                        case FetchStrategies.First:
                            index = 0;
                            DebugHelper.WriteLine("(Getting Item) Got the FIRST one!");
                            break;
                        case FetchStrategies.Last:
                            index = this._itemsList.Count - 1;
                            DebugHelper.WriteLine("(Getting Item), Got the LAST one!");
                            break;
                        default:
                            throw new Exception();
                    }
                }
                if(index == -1) {
                    return false;
                }
                item = this._itemsList[index].Item1;
                this._itemsList[index].Item2 = true;
            }
            return true;
        }

        private List<MutableTuple<T, bool>> _itemsList = new List<MutableTuple<T, bool>>();
        private object _syncRoot = new object();
    }
}
