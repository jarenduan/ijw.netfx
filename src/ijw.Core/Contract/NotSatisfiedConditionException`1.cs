using System;

namespace ijw.Contract {
    internal class NotSatisfiedConditionException<T> : ContractBrokenException {
        private T obj;
        private Predicate<T> predicate;

        public NotSatisfiedConditionException() {
        }

        public NotSatisfiedConditionException(string message) : base(message) {
        }

        public NotSatisfiedConditionException(string message, Exception innerException) : base(message, innerException) {
        }

        public NotSatisfiedConditionException(T obj, Predicate<T> predicate) {
            this.obj = obj;
            this.predicate = predicate;
        }
    }
}