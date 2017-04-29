namespace ijw.Entity {
    public class PropertyValueChangeEventArgs {
        public PropertyValueChangeEventArgs(string propertyName, object oldvalue, object newvalue) {
            this.PropertyName = propertyName;
            this.OldValue = oldvalue;
            this.NewValue = newvalue;
        }

        public string PropertyName { get; protected set; }
        public object OldValue { get; protected set; }
        public object NewValue { get; protected set; }
    }
}