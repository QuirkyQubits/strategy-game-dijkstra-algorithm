using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Script.Serialization;
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
    /// Still need to test this class.
    /// 
    /// </summary>
    /// <param name="tiles">Comes in uninitialized, and is initialzied
    /// based in the read values of ROWS and COLS in the JSON file</param>
    /// <param name="json"></param>
    public static void InitializeTiles(out Tile[,] tiles, string json)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        dynamic outerJson = serializer.Deserialize<object>(json);
        int readRows = outerJson["ROWS"];
        int readCols = outerJson["COLS"];
        dynamic readTilesArray = outerJson["tilesArray"];

        if (readRows < 1)
        {
            throw new MainClassException("MainClass::InitializeTiles(): readRows < 1");
        }
        if (readCols < 1)
        {
            throw new MainClassException("MainClass::InitializeTiles(): readCols < 1");
        }

        tiles = new Tile[readRows, readCols];

        for (int r = 0; r < readRows; ++r)
        {
            for (int c = 0; c < readCols; ++c)
            {
                dynamic tile = readTilesArray[r][c];

                if (tile["tileType"].Equals("grass"))
                {
                    tiles[r, c] = new Tile(1);
                }
                else if (tile["tileType"].Equals("water"))
                {
                    tiles[r, c] = new Tile(3);
                }
                else if (tile["tileType"].Equals("marsh"))
                {
                    tiles[r, c] = new Tile(2);
                }
                else if (tile["tileType"].Equals("plasma"))
                {
                    tiles[r, c] = new Tile(0);
                }
                else
                {
                    throw new MainClassException("MainClass::InitializeTiles(): unrecognized tileType");
                }
            }
        }
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
        int[,] dist,
        Coordinate[,] prev,
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
                    dist[coord.r, coord.c] = int.MaxValue;
                    prev[coord.r, coord.c] = null;
                }
                else
                {
                    dist[coord.r, coord.c] = 0;
                    prev[coord.r, coord.c] = null; // doesn't matter for start coord
                }
            }
        }

        while (frontier.HeapSize() > 0)
        {
            TileCoordinateNode node = frontier.Pop();
            TileCoordinate tileCoordinate = node.tileCoordinate;
            Coordinate coordinate = tileCoordinate.coordinate;

            foreach (TileCoordinate adjacentTileCoord
                in GetAdjacentTiles(tiles, coordinate))
            {
                Coordinate adjacentCoord = adjacentTileCoord.coordinate;

                // watch for overflow here
                int calculatedDist = dist[coordinate.r, coordinate.c]
                    + adjacentTileCoord.tile.movementCost;

                bool calculatedDistPreferrable
                    = dist[adjacentCoord.r, adjacentCoord.c] == int.MaxValue
                    || calculatedDist < dist[adjacentCoord.r, adjacentCoord.c];

                if (calculatedDistPreferrable && calculatedDist <= movementPoints)
                {
                    dist[adjacentCoord.r, adjacentCoord.c] = calculatedDist;
                    prev[adjacentCoord.r, adjacentCoord.c] = coordinate;

                    TileCoordinateNode adjacentNode
                        = new TileCoordinateNode(adjacentTileCoord, calculatedDist);

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
        int[,] dist,
        Coordinate[,] prev,
        Coordinate startingCoord)
    {

        // distance from starting coordinate to each node:

        for (int r = 0; r < ROWS; ++r)
        {
            for (int c = 0; c < COLS; ++c)
            {
                Coordinate targetCoordinate = new Coordinate(r, c);
                int distanceToTarget = dist[r, c];

                if (distanceToTarget != int.MaxValue)
                {
                    Console.WriteLine($"Distance from {startingCoord}" +
                        $" to {targetCoordinate}" +
                        $" is: {distanceToTarget}");
                }
            }
        }

        // find paths to starting coordinate; read backwards

        for (int r = 0; r < ROWS; ++r)
        {
            for (int c = 0; c < COLS; ++c)
            {
                Coordinate currentCoord = new Coordinate(r, c);

                if (currentCoord.Equals(startingCoord)
                    || prev[currentCoord.r, currentCoord.c] != null)
                {
                    string ans = currentCoord.ToString();

                    // all paths lead to the starting coordinate
                    // and the starting coordinate's prev is null
                    while (prev[currentCoord.r, currentCoord.c] != null)
                    {
                        ans = $"{prev[currentCoord.r, currentCoord.c]}, {ans}";
                        currentCoord = prev[currentCoord.r, currentCoord.c];
                    }

                    Console.WriteLine(ans);
                }
            }
        }
    }

    public static void Main()
    {
        Tile[,] tiles = new Tile[ROWS, COLS];
        InitializeTiles(tiles);
        Coordinate startingCoordinate = new Coordinate(2, 1);

        int[,] dist = new int[ROWS, COLS];
        Coordinate[,] prev = new Coordinate[ROWS, COLS];

        FindReachableTiles(dist, prev, tiles, startingCoordinate, 3);

        ProcessResults(dist, prev, startingCoordinate);
    }
}