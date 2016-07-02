namespace ijw.Collection {
    /// <summary>
    /// 可变的二元元祖
    /// </summary>
    /// <typeparam name="T1">第一个元素的类型</typeparam>
    /// <typeparam name="T2">第二个元素的类型</typeparam>
    public class MutableTuple<T1, T2> {
        public MutableTuple() {

        }
        /// <summary>
        /// 第一个元素
        /// </summary>
        public T1 Item1 { get; set; }

        /// <summary>
        /// 第二个元素
        /// </summary>
        public T2 Item2 { get; set; }
    }
}
