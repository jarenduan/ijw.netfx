namespace ijw {
    /// <summary>
    /// 接受ref参数的委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    public delegate void ActionWithRef<T>(ref T obj);
}
