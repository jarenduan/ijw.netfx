using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ijw.Entity.Test {
    [TestClass]
    public class EntityTest {
        [TestMethod]
        public void TestSet() {
            ForTest foo = new ForTest();
            foo.PropertyChanged += Foo_PropertyChanged;
            foo.Name = "jack";
        }

        private void Foo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            throw new NotImplementedException();
        }
    }
}
