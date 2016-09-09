namespace ijw.Net {
    public static class StringExt {
        public static bool IsIPv4Address(this string ip) {
//TODO： using rex
            string[] parts = ip.Split('.');
            if (parts.Length != 4) {
                return false;
            }
            for (int i = 0; i < parts.Length; i++) {
                int j;
                if (!int.TryParse(parts[i], out j)) {
                    return false;
                }
                if (i == 0 && (j <= 0 || j > 255)) {
                    return false;
                }
                if (i != 0 && (j < 0 || j > 255)) {
                    return false;
                }
            }

            return true;
        }
    }
}
