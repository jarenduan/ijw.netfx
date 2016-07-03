using ijw.Collection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ijw.Net.Socket
{
    public class ObjectAvailabeEventArgs<T> : EventArgs
    {
        public LongTimeConsumerCollection<T> DataPool { get; set; }
    }
}
