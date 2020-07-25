using System;
using System.Collections.Generic;
using System.Diagnostics;
using StrategyGameDjikstraAlgo;

public static class MainClass
{
    static int ROWS = 5;
    static int COLS = 5;

    private static Boolean inBounds(Coordinate coordinate)
    {
        return coordinate.r >= 0 && coordinate.r <= ROWS - 1
            && coordinate.c >= 0 && coordinate.c <= COLS - 1;
    }

    private static void InitializeTiles(Tile[,] tiles)
    {
        // row 0
        tiles[0, 0] = new Tile(3);
        tiles[0, 1] = new Tile(2);
        tiles[0, 2] = new Tile(1);
        tiles[0, 3] = new Tile(1);
        tiles[0, 4] = new Tile(1);

        // row 1
        tiles[1, 0] = new Tile(2);
        tiles[1, 1] = new Tile(1);
        tiles[1, 2] = new Tile(2);
        tiles[1, 3] = new Tile(2);
        tiles[1, 4] = new Tile(1);

        // row 2
        tiles[2, 0] = new Tile(3);
        tiles[2, 1] = new Tile(3);
        tiles[2, 2] = new Tile(1);
        tiles[2, 3] = new Tile(1);
        tiles[2, 4] = new Tile(1);

        // row 3
        tiles[3, 0] = new Tile(2);
        tiles[3, 1] = new Tile(1);
        tiles[3, 2] = new Tile(2);
        tiles[3, 3] = new Tile(2);
        tiles[3, 4] = new Tile(1);

        // row 4
        tiles[4, 0] = new Tile(3);
        tiles[4, 1] = new Tile(2);
        tiles[4, 2] = new Tile(1);
        tiles[4, 3] = new Tile(1);
        tiles[4, 4] = new Tile(1);
    }

    /// <summary>
    /// Gets all adjacent tiles in bounds.
    /// </summary>
    /// <param name="tiles"></param>
    /// <param name="startingCoordinate"></param>
    private static IEnumerable<TileCoordinate> GetAdjacentTiles(
        Tile[,] tiles,
        Coordinate coord)
    {
        Coordinate leftCoord = coord.Left();
        Coordinate downCoord = coord.Down();
        Coordinate rightCoord = coord.Right();
        Coordinate upCoord = coord.Up();

        Tile leftTile = tiles[leftCoord.r, leftCoord.c];
        Tile downTile = tiles[downCoord.r, downCoord.c];
        Tile rightTile = tiles[rightCoord.r, rightCoord.c];
        Tile upTile = tiles[upCoord.r, upCoord.c];

        yield return new TileCoordinate(leftTile, leftCoord);
        yield return new TileCoordinate(downTile, downCoord);
        yield return new TileCoordinate(rightTile, rightCoord);
        yield return new TileCoordinate(upTile, upCoord);
    }


    /// <summary>
    /// Given an array of tiles and a starting coordinate,
    /// find all reachable tiles (and paths to get to those tiles).
    /// This is implemented using a modified form of Djikstra's algorithm.
    /// </summary>
    /// <param name="tiles"></param>
    /// <param name="startingCoordinate"></param>
    private static void FindReachableTiles(Tile[,] tiles, Coordinate startingCoordinate)
    {
        var Q = new MinHeap<TileCoordinateNode>();

        var dist = new Dictionary<Coordinate, int>();
        var prev = new Dictionary<Coordinate, Coordinate>();

        dist[startingCoordinate] = 0;

        // iterate over all cells, filling the necessary tables of info
        // for proper operation of Djikstra's algorithm
        for (int r = 0; r < ROWS; ++r)
        {
            for (int c = 0; c < COLS; ++c)
            {
                Coordinate coord = new Coordinate(r, c);

                if (coord != startingCoordinate)
                {
                    dist.Add(coord, int.MaxValue); // unknown distance
                    prev.Add(coord, null); // unknown predecessor
                }

                // insert with priority

                TileCoordinate tileCoordinate = new TileCoordinate(tiles[r, c], coord);
                TileCoordinateNode node = new TileCoordinateNode(tileCoordinate, int.MaxValue);

                Q.Insert(node);


            }
        }

        while (Q.GetHeapSize() > 0)
        {
            TileCoordinateNode node = Q.Pop();
            TileCoordinate tileCoordate = node.tileCoordinate;
            
            foreach (TileCoordinate adjacent
                in GetAdjacentTiles(tiles, tileCoordate.coordinate))
            {
                int calculatedDist = dist[tileCoordate.coordinate]
                    + tileCoordate.tile.movementCost;

                if (calculatedDist < dist[tileCoordate.coordinate])
                {
                    dist[tileCoordate.coordinate] = calculatedDist;
                    prev[tileCoordate.coordinate] = adjacent.coordinate;

                    /*
                    // how to implement decrease key?
                    int heapIndexOfNode = GetHeapIndexOfNode();
                    node.totalCost = calculatedDist;
                    Q.DecreaseKey(heapIndexOfNode, node);
                    */
                }
            }
        }
    }

    public static void Main()
    {
        Tile[,] tiles = new Tile[5, 5];
        InitializeTiles(tiles);
        Coordinate startingCoordinate = new Coordinate(2, 1);
        FindReachableTiles(tiles, startingCoordinate);
    }
}