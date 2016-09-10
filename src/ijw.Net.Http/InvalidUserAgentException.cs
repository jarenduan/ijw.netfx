using System;

namespace ijw.Net.Http {
    [Serializable]
    internal class InvalidUserAgentException : Exception {
        public InvalidUserAgentException() {
        }

        public InvalidUserAgentException(string message) : base(message) {
        }

        public InvalidUserAgentException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}