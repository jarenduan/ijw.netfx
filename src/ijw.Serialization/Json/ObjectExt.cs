namespace ijw.Serialization.Json {
    /// <summary>
    /// 提供Object序列化的扩展方法
    /// </summary>
    public static class ObjectExt {
        /// <summary>
        /// 把对象序列化成Json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>输出的字符串</returns>
        public static string ToJsonString(this object obj) {
            return JsonSerializationHelper.SaveObjectToJsonString(obj);
        }

        /// <summary>
        /// 把对象序列化成带有Tab和换行的Json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>输出的字符串</returns>
        public static string ToJsonFormatString(this object obj) {
            return JsonSerializationHelper.SaveObjectToJsonString(obj).FormatJson();
        }

        public static T FromJsonString<T>(this T obj, string jsonString) {
            return JsonSerializationHelper.LoadJsonObject<T>(jsonString);
        }
    }
}
