using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGameDjikstraAlgo {
    public class Tile : IComparable<Tile>
    {
        public int movementCost;

        public Tile(int movementCost)
        {
            this.movementCost = movementCost;
        }

        public int CompareTo(Tile other)
        {
            return movementCost.CompareTo(other.movementCost);
        }
    }
}
