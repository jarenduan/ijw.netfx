using System;
using System.Threading;
using System.Threading.Tasks;

namespace ijw.Threading.Tasks {
    public static class TaskHelper {
        public static async Task Run(Action action) {
#if NET35 || NET40
            await Task.Factory.StartNew(action);
#else
            await Task.Run(action);
#endif
        }
        public static async Task Run(Action action, CancellationToken cancellationToken) {
#if NET35 || NET40
            await Task.Factory.StartNew(action, cancellationToken);
#else
            await Task.Run(action, cancellationToken);
#endif
        }
    }
}
