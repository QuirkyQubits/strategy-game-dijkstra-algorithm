using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGameDjikstraAlgo
{
    public class Coordinate
    {
        public int r;
        public int c;

        public Coordinate(int r, int c)
        {
            this.r = r;
            this.c = c;
        }

        public Coordinate Left()
        {
            return new Coordinate(r, c - 1);
        }

        public Coordinate Right()
        {
            return new Coordinate(r, c + 1);
        }

        public Coordinate Up()
        {
            return new Coordinate(r - 1, c);
        }

        public Coordinate Down()
        {
            return new Coordinate(r + 1, c);
        }

        public override int GetHashCode()
        {
            // Cantor pairing function on non-negative numbers

            return (r + c) * (r + c + 1) / 2 + c;

            // may want to account for
            // 1. negative numbers
            // 2. overflow for int.MaxValue
        }
        public override bool Equals(Object obj)
        {
            Coordinate item = obj as Coordinate;

            return item != null && r == item.r && c == item.c;
        }

        public override string ToString()
        {
            return $"({r}, {c})";
        }
    }
}
