﻿using Microsoft.Win32;
using System;
using System.Globalization;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace ijw.Client.Win32 {
    public sealed class InputLanguage {
        // Fields
        private readonly IntPtr handle;

        // Methods
        internal InputLanguage(IntPtr handle) {
            this.handle = handle;
        }

        public override bool Equals(object value) {
            return ((value is InputLanguage) && (this.handle == ((InputLanguage)value).handle));
        }

        public static InputLanguage FromCulture(CultureInfo culture) {
            int keyboardLayoutId = culture.KeyboardLayoutId;
            foreach (InputLanguage language in InstalledInputLanguages) {
                if ((((int)((long)language.handle)) & 0xffff) == keyboardLayoutId) {
                    return language;
                }
            }
            return null;
        }

        public override int GetHashCode() {
            return (int)((long)this.handle);
        }

        private static string GetLocalizedKeyboardLayoutName(string layoutDisplayName) {
            if ((layoutDisplayName != null) && (Environment.OSVersion.Version.Major >= 5)) {
                StringBuilder pszOutBuf = new StringBuilder(0x200);
                if (Win32API.SHLoadIndirectString(layoutDisplayName, pszOutBuf, (uint)pszOutBuf.Capacity, IntPtr.Zero) == 0) {
                    return pszOutBuf.ToString();
                }
            }
            return null;
        }
           public static InputLanguageCollection InstalledInputLanguages
        {
            get
            {
                int keyboardLayoutList = Win32KeyBoardLayout.GetKeyboardLayoutList(0, null);
                IntPtr[] hkls = new IntPtr[keyboardLayoutList];
                Win32KeyBoardLayout.GetKeyboardLayoutList(keyboardLayoutList, hkls);
                InputLanguage[] languageArray = new InputLanguage[keyboardLayoutList];
                for (int i = 0; i < keyboardLayoutList; i++) {
                    languageArray[i] = new InputLanguage(hkls[i]);
                }
                return new InputLanguageCollection(languageArray);
            }
        }

        private static string PadWithZeroes(string input, int length) {
            return ("0000000000000000".Substring(0, length - input.Length) + input);
        }

        // Properties
        public CultureInfo Culture
        {
            get
            {
                return new CultureInfo(((int)this.handle) & 0xffff);
            }
        }

        public static InputLanguage CurrentInputLanguage {
            get {
                //Application.OleRequired();
                return new InputLanguage(Win32KeyBoardLayout.GetKeyboardLayout(0));
            }
            //set {
            //    IntSecurity.AffectThreadBehavior.Demand();
            //    Application.OleRequired();
            //    if (value == null) {
            //        value = DefaultInputLanguage;
            //    }
            //    if (SafeNativeMethods.ActivateKeyboardLayout(new HandleRef(value, value.handle), 0) == IntPtr.Zero) {
            //        throw new ArgumentException(SR.GetString("ErrorBadInputLanguage"), "value");
            //    }
            //}
        }

        public static InputLanguage DefaultInputLanguage
        {
            get
            {
                IntPtr[] rc = new IntPtr[1];
                Win32API.SystemParametersInfo(0x59, 0, rc, 0);
                return new InputLanguage(rc[0]);
            }
        }

        public IntPtr Handle
        {
            get
            {
                return this.handle;
            }
        }

        public string LayoutName
        {
            get
            {
                string localizedKeyboardLayoutName = null;
                IntPtr handle = this.handle;
                int num = ((int)((long)handle)) & 0xffff;
                int num2 = (((int)((long)handle)) >> 0x10) & 0xfff;
                new RegistryPermission(PermissionState.Unrestricted).Assert();
                try {
                    if ((num2 == num) || (num2 == 0)) {
                        RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Keyboard Layouts\" + PadWithZeroes(Convert.ToString(num, 0x10), 8));
                        localizedKeyboardLayoutName = GetLocalizedKeyboardLayoutName(key.GetValue("Layout Display Name") as string);
                        if (localizedKeyboardLayoutName == null) {
                            localizedKeyboardLayoutName = (string)key.GetValue("Layout Text");
                        }
                        key.Close();
                    }
                    else {
                        RegistryKey key2 = Registry.CurrentUser.OpenSubKey(@"Keyboard Layout\Substitutes");
                        string[] valueNames = null;
                        if (key2 != null) {
                            valueNames = key2.GetValueNames();
                            foreach (string str3 in valueNames) {
                                int num4 = Convert.ToInt32(str3, 0x10);
                                if (((num4 == ((int)((long)handle))) || ((num4 & 0xfffffff) == (((int)((long)handle)) & 0xfffffff))) || ((num4 & 0xffff) == num)) {
                                    handle = (IntPtr)Convert.ToInt32((string)key2.GetValue(str3), 0x10);
                                    num = ((int)((long)handle)) & 0xffff;
                                    num2 = (((int)((long)handle)) >> 0x10) & 0xfff;
                                    break;
                                }
                            }
                            key2.Close();
                        }
                        RegistryKey key3 = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Keyboard Layouts");
                        if (key3 != null) {
                            valueNames = key3.GetSubKeyNames();
                            foreach (string str4 in valueNames) {
                                if (handle == ((IntPtr)Convert.ToInt32(str4, 0x10))) {
                                    RegistryKey key4 = key3.OpenSubKey(str4);
                                    localizedKeyboardLayoutName = GetLocalizedKeyboardLayoutName(key4.GetValue("Layout Display Name") as string);
                                    if (localizedKeyboardLayoutName == null) {
                                        localizedKeyboardLayoutName = (string)key4.GetValue("Layout Text");
                                    }
                                    key4.Close();
                                    break;
                                }
                            }
                        }
                        if (localizedKeyboardLayoutName == null) {
                            foreach (string str5 in valueNames) {
                                if (num == (0xffff & Convert.ToInt32(str5.Substring(4, 4), 0x10))) {
                                    RegistryKey key5 = key3.OpenSubKey(str5);
                                    string str6 = (string)key5.GetValue("Layout Id");
                                    if ((str6 != null) && (Convert.ToInt32(str6, 0x10) == num2)) {
                                        localizedKeyboardLayoutName = GetLocalizedKeyboardLayoutName(key5.GetValue("Layout Display Name") as string);
                                        if (localizedKeyboardLayoutName == null) {
                                            localizedKeyboardLayoutName = (string)key5.GetValue("Layout Text");
                                        }
                                    }
                                    key5.Close();
                                    if (localizedKeyboardLayoutName != null) {
                                        break;
                                    }
                                }
                            }
                        }
                        key3.Close();
                    }
                }
                finally {
                    CodeAccessPermission.RevertAssert();
                }
                if (localizedKeyboardLayoutName == null) {
                    //localizedKeyboardLayoutName = SR.GetString("UnknownInputLanguageLayout");
                    localizedKeyboardLayoutName = "UnknownInputLanguageLayout";
                }
                return localizedKeyboardLayoutName;
            }
        }
    }

}
