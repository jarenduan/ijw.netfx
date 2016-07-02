using System;

namespace ijw
{
    public static class Helper
    {
        public static void PythonStartEndCalculator(int length, out int startAtPython, out int endAtPython, int? startAt = 0, int? endAt = null) {
            if (startAt == null) {
                startAt = 0;
            }
            else if (startAt < 0) {
                startAt = length + startAt;
            }


            if (endAt == null) {
                endAt = length;
            }
            else if (endAt < 0) {
                endAt = length + endAt;
            }

            startAtPython = startAt.Value;
            endAtPython = endAt.Value;
        }
    }
}
