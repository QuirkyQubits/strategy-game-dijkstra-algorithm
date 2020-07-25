using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGameDjikstraAlgo
{
    public class TileCoordinate : IComparable<TileCoordinate>
    {
        public TileCoordinate(Tile tile, Coordinate coordinate)
        {
            this.tile = tile;
            this.coordinate = coordinate;
        }

        public Tile tile;
        public Coordinate coordinate;

        // Basically compares the movement costs.
        public int CompareTo(TileCoordinate other)
        {
            return this.tile.CompareTo(other.tile);
        }
    }
}
