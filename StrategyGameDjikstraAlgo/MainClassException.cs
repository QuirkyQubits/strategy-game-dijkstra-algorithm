using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGameDjikstraAlgo
{
    [Serializable()]
    public class MainClassException : System.Exception
    {
        public MainClassException() : base() { }
        public MainClassException(string message) : base(message) { }
        public MainClassException(string message, System.Exception inner)
            : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected MainClassException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
