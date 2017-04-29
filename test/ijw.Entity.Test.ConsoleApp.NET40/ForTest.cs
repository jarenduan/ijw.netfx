using System;

namespace ijw.Entity.Test {
    internal class ForTest : EntityBase {
        private string _name;

        public string Name {
            get => this._name;
            set => this.Set(ref this._name, value);
        }
    }
}
