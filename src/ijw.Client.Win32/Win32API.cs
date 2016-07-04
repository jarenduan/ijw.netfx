using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ijw.Client.Win32 {
    public class Win32API {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern uint SHLoadIndirectString(string pszSource, StringBuilder pszOutBuf, uint cchOutBuf, IntPtr ppvReserved);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SystemParametersInfo(int nAction, int nParam, [In, Out] IntPtr[] rc, int nUpdate);

    }
}
