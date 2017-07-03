using ijw.Diagnostic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using ijw.Contract;

namespace ijw.Entity {
    public abstract class NotifyPropertyChangeBase : INotifyPropertyChanged, INotifyPropertyChanging, IWhenPropertyChanges {
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;
        public event BeforePropertyChangingEventHandler BeforePropertyValueChanges;
        public event AfterPropertyChangedEventHandler AfterPropertyValueChanged;
#if NETSTANDARD1_4
        protected virtual void Set<T>(ref T property, T value, [CallerMemberName]string propertyName = null) {
            propertyName.ShouldBeNotNullArgument();
#else
        protected void Set<T>(ref T property, T value) {
            string propertyName = DebugHelper.GetCallerMethod().Name.Remove(0, 4);
#endif
            if (!object.Equals(property, value)) {
                if (this.BeforePropertyValueChanges != null || this.AfterPropertyValueChanged != null) {
                    var oldvalue = property;
                    var evntArgs = new PropertyValueChangeEventArgs(propertyName, oldvalue, value);
                    bool allowChanging = raiseBeforePropertyValueChangesEvent(evntArgs);
                    if (!allowChanging) return;
                    setvalue(ref property, value, propertyName);
                    raiseAfterPropertyValueChangesEvent(evntArgs);
                }
                else {
                    setvalue(ref property, value, propertyName);
                }
            }
        }

        private bool raiseBeforePropertyValueChangesEvent(PropertyValueChangeEventArgs evntArgs) {
            var evntHndlrs = this.BeforePropertyValueChanges.GetInvocationList();
            foreach (var evntHndlr in evntHndlrs) {
                var handler = (BeforePropertyChangingEventHandler)evntHndlr;
                var result = handler(this, evntArgs);
                if (result == false) return false;
            }
            return true;
        }

        private void raiseAfterPropertyValueChangesEvent(PropertyValueChangeEventArgs evntArgs) {
            this.AfterPropertyValueChanged?.Invoke(this, evntArgs);
        }

        private void setvalue<T>(ref T property, T value, string propertyName) {
            this.PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
            property = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
