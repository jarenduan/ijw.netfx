using System.Collections;

namespace ijw.Client.Win32 {

    public class InputLanguageCollection : ReadOnlyCollectionBase {
        // Methods
        internal InputLanguageCollection(InputLanguage[] value) {
            base.InnerList.AddRange(value);
        }

        public bool Contains(InputLanguage value) {
            return base.InnerList.Contains(value);
        }

        public void CopyTo(InputLanguage[] array, int index) {
            base.InnerList.CopyTo(array, index);
        }

        public int IndexOf(InputLanguage value) {
            return base.InnerList.IndexOf(value);
        }

        // Properties
        public InputLanguage this[int index]
        {
            get
            {
                return (InputLanguage)base.InnerList[index];
            }
        }
    }
}