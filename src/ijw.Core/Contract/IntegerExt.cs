namespace ijw.Contract {
    public static class IntegerExt {
        public static bool ShouldBeNotZero(this int obj) {
            return obj.ShouldNotEquals(0);
        }

        public static bool ShouldBeNotLessThanZero(this int obj) {
            return obj.ShouldNotLessThan(0);
        }

        public static bool ShouldBeEven(this int obj) {
            if (obj % 2 != 0) {
                throw new ContractBrokenException();
            }
            return true;
        }

        public static bool ShouldBeOdd(this int obj) {
            if (obj % 2 == 0) {
                throw new  ContractBrokenException();
            }
            return true;
        }

        
    }
}
