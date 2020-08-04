using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using StrategyGameDjikstraAlgo;

public static class GameFunctions
{
    public static bool inBounds(Coordinate coordinate, int rows, int cols)
    {
        return coordinate.r >= 0 && coordinate.r <= rows - 1
            && coordinate.c >= 0 && coordinate.c <= cols - 1;
    }

    public static bool inBounds<T>(Coordinate coordinate, T[,] tiles)
    {
        int rows = Util.GetRows(tiles);
        int cols = Util.GetCols(tiles);

        return inBounds(coordinate, rows, cols);
    }

    [Obsolete("Use the json method instead")]
    /// <summary>
    /// Assumes a 5x5 board, and initialzies the wtiles with certain values.
    /// </summary>
    /// <param name="tiles"></param>
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
    /// Returns an array of Tiles based on the json string parameter.
    /// </summary>
    /// <param name="tiles">Comes in uninitialized, and is initialzied
    /// based in the read values of ROWS and COLS in the JSON file</param>
    /// <param name="json"></param>
    public static Tile[,] GetTiles(string json)
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

        Tile[,] tiles = new Tile[readRows, readCols];

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

        return tiles;
    }

    /// <summary>
    /// Gets all adjacent tiles in bounds.
    /// </summary>
    /// <param name="tiles"></param>
    /// <param name="startingCoordinate"></param>
    public static IEnumerable<TileCoordinate> GetAdjacentTiles(
        Tile[,] tiles,
        Coordinate coord)
    {
        const string errorMessage
                = "MainClass::GetAdjacentTiles(): Specified coordinate not in bounds";

        int rows = Util.GetRows(tiles);
        int cols = Util.GetCols(tiles);

        if (!inBounds(coord, tiles))
            throw new MainClassException(errorMessage);

        Debug.Assert(inBounds(coord, tiles), errorMessage);

        Coordinate leftCoord = coord.Left();
        Coordinate downCoord = coord.Down();
        Coordinate rightCoord = coord.Right();
        Coordinate upCoord = coord.Up();

        Tile leftTile = inBounds(leftCoord,   tiles) ? tiles[leftCoord.r, leftCoord.c] : null;
        Tile downTile = inBounds(downCoord,   tiles) ? tiles[downCoord.r, downCoord.c] : null;
        Tile rightTile = inBounds(rightCoord, tiles) ? tiles[rightCoord.r, rightCoord.c] : null;
        Tile upTile = inBounds(upCoord,       tiles) ? tiles[upCoord.r, upCoord.c] : null;

        if (inBounds(leftCoord, tiles))
            yield return new TileCoordinate(leftTile, leftCoord);

        if (inBounds(downCoord, tiles))
            yield return new TileCoordinate(downTile, downCoord);

        if (inBounds(rightCoord, tiles))
            yield return new TileCoordinate(rightTile, rightCoord);

        if (inBounds(upCoord, tiles))
            yield return new TileCoordinate(upTile, upCoord);
    }


    /// <summary>
    /// Gets all adjacent tiles in bounds,
    /// and depending on which team it is,
    /// allows only coordinates with the other teams to be selected.
    /// </summary>
    /// <param name="tiles"></param>
    /// <param name="startingCoordinate"></param>
    public static IEnumerable<TileCoordinate> GetAllowableAdjacentTiles(
        Tile[,] tiles,
        Coordinate coord,
        Unit[,] board,
        Teams currentTeam)
    {
        foreach (TileCoordinate tileCoord in GetAdjacentTiles(tiles, coord))
        {
            Coordinate target = tileCoord.coordinate;

            if (board[target.r, target.c] is null
                || currentTeam == board[target.r, target.c].team)
            {
                yield return tileCoord;
            }
        }
    }


    [Obsolete("This method is deprecated, " +
        "please use the overlaod with Unit parameter instead")]
    /// <summary>
    /// Given an array of tiles and a starting coordinate,
    /// find all reachable tiles (and paths to get to those tiles).
    /// This is implemented using a modified form of Djikstra's algorithm.
    /// </summary>
    /// <param name="tiles"></param>
    /// <param name="startCoord"></param>
    public static void FindReachableTiles(
        int rows,
        int cols,
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

        for (int r = 0; r < rows; ++r)
        {
            for (int c = 0; c < cols; ++c)
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


    /// <summary>
    /// Given an array of tiles and a starting coordinate,
    /// find all reachable tiles (and paths to get to those tiles).
    /// This is implemented using a modified form of Djikstra's algorithm.
    /// 
    /// We return a dictionary of <option, tuple<cost-to-option, path-to-option>>.
    /// </summary>
    /// <param name="tiles"></param>
    /// <param name="startCoord"></param>
    public static Dictionary<Coordinate, Tuple<int, List<Coordinate>>> FindReachableTiles(
        Unit unit,
        Tile[,] tiles)
    {
        int rows = Util.GetRows(tiles);
        int cols = Util.GetCols(tiles);

        int[,] dist = new int[rows, cols];
        Coordinate[,] prev = new Coordinate[rows, cols];

        var frontier = new MinHeap<TileCoordinateNode>();

        TileCoordinate startTileCoordinate = new TileCoordinate(
            tiles[unit.location.r, unit.location.c],
            unit.location);

        TileCoordinateNode startNode = new TileCoordinateNode(startTileCoordinate, 0);

        frontier.Insert(startNode);

        for (int r = 0; r < rows; ++r)
        {
            for (int c = 0; c < cols; ++c)
            {
                Coordinate coord = new Coordinate(r, c);

                if (!coord.Equals(unit.location))
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
                in GetAllowableAdjacentTiles(tiles, coordinate, unit.board, unit.team))
            {
                Coordinate adjacentCoord = adjacentTileCoord.coordinate;

                // watch for overflow here
                int calculatedDist = dist[coordinate.r, coordinate.c]
                    + adjacentTileCoord.tile.movementCost;

                bool calculatedDistPreferrable
                    = dist[adjacentCoord.r, adjacentCoord.c] == int.MaxValue
                    || calculatedDist < dist[adjacentCoord.r, adjacentCoord.c];

                if (calculatedDistPreferrable && calculatedDist <= unit.movementPoints)
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

        // djikstra finished
        // now processing and adding to the return dict

        var answer = new Dictionary<Coordinate, Tuple<int, List<Coordinate>>>();

        for (int r = 0; r < rows; ++r)
        {
            for (int c = 0; c < cols; ++c)
            {
                Coordinate targetCoord = new Coordinate(r, c);
                int distanceToTarget = dist[r, c];

                // cell must also be empty, unless it is the starting coord
                if (distanceToTarget != int.MaxValue
                    && (targetCoord.Equals(unit.location)
                    || unit.board[targetCoord.r, targetCoord.c] is null))
                {
                    /*
                    Console.WriteLine($"Distance from {unit.currentLocation}" +
                        $" to {targetCoordinate}" +
                        $" is: {distanceToTarget}");
                    */

                    //string ans = targetCoordinate.ToString();
                    List<Coordinate> pathToTarget = new List<Coordinate>();

                    Coordinate currentCoord = targetCoord;

                    // all paths lead to the starting coordinate
                    // and the starting coordinate's prev is null
                    while (prev[currentCoord.r, currentCoord.c] != null)
                    {
                        // ans = $"{prev[targetCoordinate.r, targetCoordinate.c]}, {ans}";
                        pathToTarget.Insert(0, prev[currentCoord.r, currentCoord.c]);

                        currentCoord = prev[currentCoord.r, currentCoord.c];
                    }

                    pathToTarget.Add(targetCoord);

                    // Console.WriteLine($"path to {targetCoord}: {String.Join(", ", pathToTarget)}");

                    answer.Add(
                        targetCoord,
                        new Tuple<int, List<Coordinate>>(
                            distanceToTarget,
                            pathToTarget)
                        );
                }
            }
        }

        return answer;
    }


    public static void ProcessResults(
        int rows,
        int cols,
        int[,] dist,
        Coordinate[,] prev,
        Coordinate startingCoord)
    {

        // distance from starting coordinate to each node:

        for (int r = 0; r < rows; ++r)
        {
            for (int c = 0; c < cols; ++c)
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

        for (int r = 0; r < rows; ++r)
        {
            for (int c = 0; c < cols; ++c)
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

    public static void MainMethod1()
    {
        int ROWS = 5;
        int COLS = 5;

        Tile[,] tiles = new Tile[ROWS, COLS];
        InitializeTiles(tiles);
        Coordinate startingCoordinate = new Coordinate(2, 1);

        int[,] dist = new int[ROWS, COLS];
        Coordinate[,] prev = new Coordinate[ROWS, COLS];

        FindReachableTiles(ROWS, COLS, dist, prev, tiles, startingCoordinate, 3);

        ProcessResults(ROWS, COLS, dist, prev, startingCoordinate);
    }

    public static HashSet<Coordinate> FindAttackableTiles(
        Unit unit,
        Unit[,] board)
    {
        HashSet<Coordinate> answer = new HashSet<Coordinate>();

        foreach (Coordinate adjCoord in GetAdjacentCoordinates(board, unit.location))
        {
            Unit adjacentUnit = board[adjCoord.r, adjCoord.c];

            if (adjacentUnit != null && adjacentUnit.team != unit.team)
                answer.Add(adjCoord);
        }

        return answer;
    }


    private static IEnumerable<Coordinate> GetAdjacentCoordinates(
        Unit[,] board,
        Coordinate coord)
    {
        const string errorMessage
        = "MainClass::GetAdjacentCoordinates(): Specified coordinate not in bounds";

        int rows = Util.GetRows(board);
        int cols = Util.GetCols(board);

        if (!inBounds(coord, board))
            throw new MainClassException(errorMessage);

        Debug.Assert(inBounds(coord, board), errorMessage);

        Coordinate leftCoord = coord.Left();
        Coordinate downCoord = coord.Down();
        Coordinate rightCoord = coord.Right();
        Coordinate upCoord = coord.Up();

        if (inBounds(leftCoord, board))
            yield return leftCoord;

        if (inBounds(downCoord, board))
            yield return downCoord;

        if (inBounds(rightCoord, board))
            yield return rightCoord;

        if (inBounds(upCoord, board))
            yield return upCoord;
    }

    private static void RunTestSequence(
        string pathToJsonFile,
        Coordinate startCoord,
        int movementPoints)
    {
        string json = File.ReadAllText(pathToJsonFile);

        Tile[,] tiles = GetTiles(json);

        int rows = Util.GetRows(tiles);
        int cols = Util.GetCols(tiles);

        int[,] dist = new int[rows, cols];
        Coordinate[,] prev = new Coordinate[rows, cols];

        FindReachableTiles(rows, cols, dist, prev, tiles, startCoord, movementPoints);

        ProcessResults(rows, cols, dist, prev, startCoord);
    }

    private static void Method1ToRunTestSequence()
    {
        string pathToExportFolder = Util.GetPathToExportFolder();
        string pathToJsonFile = $@"{pathToExportFolder}\strategy-game-export-4.json";

        RunTestSequence(
            pathToJsonFile,
            new Coordinate(1, 0),
            3);
    }
}