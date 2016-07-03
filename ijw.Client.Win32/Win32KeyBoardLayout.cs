using System;
using System.Runtime.InteropServices;

namespace ijw.Client.Win32 {
    public static class Win32KeyBoardLayout {
        #region KLF_Const
        /// <summary>
        /// If the specified input locale identifier is not already loaded, the function loads and activates the input locale identifier for the current thread
        /// </summary>
        public static uint KLF_ACTIVATE = 0x00000001;
        /// <summary>
        /// Substitutes the specified input locale identifier with another locale preferred by the user. The system starts with this flag set, and it is recommended that your application always use this flag
        /// </summary>
        public static uint KLF_SUBSTITUTE_OK = 0x00000002;
        /// <summary>
        /// If this bit is set, the system's circular list of loaded locale identifiers is reordered by moving the locale identifier to the head of the list. If this bit is not set, the list is rotated without a change of order
        /// </summary>
        public static uint KLF_REORDER = 0x00000008;
        /// <summary>
        /// If the new input locale identifier has the same language identifier as a current input locale identifier, the new input locale identifier replaces the current one as the input locale identifier for that language
        /// </summary>
        public static uint KLF_REPLACELANG = 0x00000010;
        /// <summary>
        /// Prevents a ShellProc hook procedure from receiving an HSHELL_LANGUAGE hook code when the new input locale identifier is loaded. This value is typically used when an application loads multiple input locale identifiers one after another
        /// </summary>
        public static uint KLF_NOTELLSHELL = 0x00000080;
        /// <summary>
        /// Activates the specified locale identifier for the entire process
        /// </summary>
        public static uint KLF_SETFORPROCESS = 0x00000100;
        /// <summary>
        /// This is used with KLF_RESET.
        /// </summary>
        public static uint KLF_SHIFTLOCK = 0x00010000;
        /// <summary>
        /// If set but KLF_SHIFTLOCK is not set, the Caps Lock state is turned off by pressing the Caps Lock key again. If set and KLF_SHIFTLOCK is also set, the Caps Lock state is turned off by pressing either SHIFT key
        /// </summary>
        public static uint KLF_RESET = 0x40000000; 
        #endregion

        /// <summary>Sets the input locale identifier (formerly called the keyboard layout handle) for the calling thread or the current process. The input locale identifier specifies a locale as well as the physical layout of the keyboard</summary>
        /// <param name="hkl">Input locale identifier to be activated.</param>
        /// <param name="Flags">Specifies how the input locale identifier is to be activated.</param>
        /// <returns>The return value is of type HKL. If the function succeeds, the return value is the previous input locale identifier. Otherwise, it is zero</returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern HKL ActivateKeyboardLayout(HKL hkl, uint Flags);

        [DllImport("imm32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int GetKeyboardLayoutList(int size, [Out, MarshalAs(UnmanagedType.LPArray)] IntPtr[] hkls);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetKeyboardLayout(int dwLayout);
    }
}
