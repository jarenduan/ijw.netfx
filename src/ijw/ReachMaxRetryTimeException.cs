using System;

namespace ijw {
    public class ReachMaxRetryTimeException : Exception {
        public ReachMaxRetryTimeException() {
        }

        public ReachMaxRetryTimeException(string message) : base(message) {
        }

        public ReachMaxRetryTimeException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}