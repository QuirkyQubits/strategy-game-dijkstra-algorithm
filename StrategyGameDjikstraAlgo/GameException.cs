using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGameDjikstraAlgo
{
    [Serializable()]
    public class GameException : System.Exception
    {
        public GameException() : base() { }
        public GameException(string message) : base(message) { }
        public GameException(string message, System.Exception inner)
            : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected GameException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
