using System;
using System.Collections.Generic;
using System.Linq;

namespace ijw.Contract
{
    public class ContractBrokenException : Exception
    {
        public ContractBrokenException(string message = ""):base (message) {
        }

        public ContractBrokenException(string message, Exception innerException) : base(message, innerException) {
        }

    }
}
