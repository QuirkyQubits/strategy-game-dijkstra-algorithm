﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGameDjikstraAlgo
{
    [Serializable()]
    public class GameFunctionException : System.Exception
    {
        public GameFunctionException() : base() { }
        public GameFunctionException(string message) : base(message) { }
        public GameFunctionException(string message, System.Exception inner)
            : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected GameFunctionException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
