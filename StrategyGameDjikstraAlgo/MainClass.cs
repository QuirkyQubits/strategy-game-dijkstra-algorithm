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
        const string errorMessage
                = "MainClass::GetAdjacentTiles(): Specified coordinate not in bounds";

        if (!inBounds(coord))
            throw new MainClassException(errorMessage);

        Debug.Assert(inBounds(coord), errorMessage);

        Coordinate leftCoord = coord.Left();
        Coordinate downCoord = coord.Down();
        Coordinate rightCoord = coord.Right();
        Coordinate upCoord = coord.Up();

        Tile leftTile = inBounds(leftCoord) ? tiles[leftCoord.r, leftCoord.c] : null;
        Tile downTile = inBounds(downCoord) ? tiles[downCoord.r, downCoord.c] : null;
        Tile rightTile = inBounds(rightCoord) ? tiles[rightCoord.r, rightCoord.c] : null;
        Tile upTile = inBounds(upCoord) ? tiles[upCoord.r, upCoord.c] : null;

        if (inBounds(leftCoord))
            yield return new TileCoordinate(leftTile, leftCoord);

        if (inBounds(downCoord))
            yield return new TileCoordinate(downTile, downCoord);

        if (inBounds(rightCoord))
            yield return new TileCoordinate(rightTile, rightCoord);

        if (inBounds(upCoord))
            yield return new TileCoordinate(upTile, upCoord);
    }


    /// <summary>
    /// Given an array of tiles and a starting coordinate,
    /// find all reachable tiles (and paths to get to those tiles).
    /// This is implemented using a modified form of Djikstra's algorithm.
    /// </summary>
    /// <param name="tiles"></param>
    /// <param name="startCoord"></param>
    private static void FindReachableTiles(
        Dictionary<Coordinate, int> dist,
        Dictionary<Coordinate, Coordinate> prev,
        Tile[,] tiles,
        Coordinate startCoord,
        int movementPoints)
    {
        var frontier = new MinHeap<TileCoordinateNode>();

        TileCoordinate startTileCoordinate = new TileCoordinate(
            tiles[startCoord.r, startCoord.c],
            startCoord);

        TileCoordinateNode startNode = new TileCoordinateNode(startTileCoordinate, 0);

        frontier.Insert(startNode);

        for (int r = 0; r < ROWS; ++r)
        {
            for (int c = 0; c < COLS; ++c)
            {
                Coordinate coord = new Coordinate(r, c);

                if (!coord.Equals(startCoord))
                {
                    dist[coord] = int.MaxValue;
                    prev[coord] = null;
                }
                else
                {
                    dist[startCoord] = 0;
                    prev[startCoord] = null; // doesn't matter for start coord
                }
            }
        }

        while (frontier.HeapSize() > 0)
        {
            TileCoordinateNode node = frontier.Pop();
            TileCoordinate tileCoordinate = node.tileCoordinate;

            foreach (TileCoordinate adjacent
                in GetAdjacentTiles(tiles, tileCoordinate.coordinate))
            {
                // watch for overflow here
                int calculatedDist = dist[tileCoordinate.coordinate]
                    + adjacent.tile.movementCost;

                bool calculatedDistPreferrable
                    = dist[adjacent.coordinate] == int.MaxValue
                    || calculatedDist < dist[adjacent.coordinate];

                if (calculatedDistPreferrable && calculatedDist <= movementPoints)
                {
                    dist[adjacent.coordinate] = calculatedDist;
                    prev[adjacent.coordinate] = tileCoordinate.coordinate;

                    TileCoordinateNode adjacentNode
                        = new TileCoordinateNode(adjacent, calculatedDist);

                    if (!frontier.Contains(adjacentNode))
                    {
                        frontier.Insert(adjacentNode);
                    }
                    else
                    {
                        frontier.DecreaseKey(adjacentNode, adjacentNode);
                    }
                }
            }
            
        }
    }

    private static void ProcessResults(
        Dictionary<Coordinate, int> dist,
        Dictionary<Coordinate, Coordinate> prev,
        Coordinate startingCoord)
    {

        // distance from starting coordinate to each node:

        foreach (KeyValuePair<Coordinate, int> entry in dist)
        {
            Coordinate targetCoordinate = entry.Key;
            int distanceToTarget = entry.Value;

            if (distanceToTarget != int.MaxValue)
            {
                Console.WriteLine($"Distance from {startingCoord}" +
                    $" to {targetCoordinate}" +
                    $" is: {distanceToTarget}");
            }
        }

        // find paths to starting coordinate; read backwards

        foreach (KeyValuePair<Coordinate, Coordinate> entry in prev)
        {
            Coordinate currentCoord = entry.Key;

            if (currentCoord.Equals(startingCoord) || prev[currentCoord] != null)
            {
                string ans = currentCoord.ToString();

                // all paths lead to the starting coordinate
                // and the starting coordinate's prev is null
                while (prev[currentCoord] != null)
                {
                    ans = $"{prev[currentCoord]}, {ans}";
                    currentCoord = prev[currentCoord];
                }

                Console.WriteLine(ans);
            }
        }
    }

    public static void Main()
    {
        Tile[,] tiles = new Tile[ROWS, COLS];
        InitializeTiles(tiles);
        Coordinate startingCoordinate = new Coordinate(2, 1);

        var dist = new Dictionary<Coordinate, int>();
        var prev = new Dictionary<Coordinate, Coordinate>();

        FindReachableTiles(dist, prev, tiles, startingCoordinate, 3);

        ProcessResults(dist, prev, startingCoordinate);
    }
}