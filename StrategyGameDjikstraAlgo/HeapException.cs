using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGameDjikstraAlgo
{
    [Serializable()]
    public class HeapException : System.Exception
    {
        public HeapException() : base() { }
        public HeapException(string message) : base(message) { }
        public HeapException(string message, System.Exception inner)
            : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected HeapException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
