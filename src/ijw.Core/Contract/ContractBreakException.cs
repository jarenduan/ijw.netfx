using System;
using System.Collections.Generic;
using System.Linq;

namespace ijw.Contract
{
    public class ContractBreakException : Exception
    {
        public ContractBreakException(string message = ""):base (message) {
        }

        public ContractBreakException(string message, Exception innerException) : base(message, innerException) {
        }

    }
}
