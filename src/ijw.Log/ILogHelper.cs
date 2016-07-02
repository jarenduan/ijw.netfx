using System;

namespace ijw.Log {
    public interface ILogHelper {
        void WriteError(string msg);
        void WriteError(Exception ex);
        void WriteInfo(string msg);
        void WriteInfo(Exception ex);
    }
}