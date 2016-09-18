using ijw.Contract;

namespace ijw.Net.Contracts {
    public static class StringExt {
        public static bool ShouldBeIPv4Address(this string aString) {
            var r = aString.IsIPv4Address();
            if (!r) {
                return false;
            }
            else {
                throw new ContractBrokenException($"{aString} is not a IPv4 Address");
            }
        }
    }
}
