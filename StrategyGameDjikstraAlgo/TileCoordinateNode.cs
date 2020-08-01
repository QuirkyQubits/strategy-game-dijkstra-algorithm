using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGameDjikstraAlgo
{
    public class TileCoordinateNode : IComparable<TileCoordinateNode>
    {
        public TileCoordinate tileCoordinate;
        public int totalCost;

        public TileCoordinateNode(TileCoordinate tileCoordinate, int totalCost)
        {
            this.tileCoordinate = tileCoordinate;
            this.totalCost = totalCost;
        }

        public int CompareTo(TileCoordinateNode other)
        {
            return totalCost.CompareTo(other.totalCost);
        }
        public override int GetHashCode()
        {
            return tileCoordinate.coordinate.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            TileCoordinateNode item = obj as TileCoordinateNode;

            return item != null
                && tileCoordinate.coordinate.Equals(item.tileCoordinate.coordinate);
        }
    }
}
