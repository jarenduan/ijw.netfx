namespace ijw.Collection {
    /// <summary>
    /// 接受一个Ref对象和行列参数的action委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="rowIndex"></param>
    /// <param name="columnIndex"></param>
    public delegate void ActionWithRefAndIndex<T>(ref T obj, int rowIndex, int columnIndex);
}
