# 🧠 Strategy Game Simulation Engine (Dijkstra Reference)

This project is a minimal, console-based simulation demonstrating how to implement **grid-based movement and turn-based combat** for a strategy game. It acts as a **reference implementation** for consuming exported JSON maps from the [Strategy Game Level Creator](https://github.com/QuirkyQubits/strategy-game-tool). It showcases how to:

- Parse terrain-based map data from JSON
- Use Dijkstra’s algorithm to compute valid movement tiles
- Simulate basic turn-based combat between units

---

## 🌟 Purpose

This simulation was built as a **proof of concept** to validate the JSON export format from the frontend level editor. It also demonstrates how this movement logic could be integrated into a game engine like Unity or Unreal.

Rather than being a full-featured engine, it focuses on:

- Terrain-aware movement calculations
- Grid placement and unit movement
- A basic turn-based combat loop

---

## 🔍 Key Features

- ✅ Reads a terrain grid from JSON (terrain type + movement cost)
- ✅ Implements Dijkstra’s algorithm for movement pathfinding
- ✅ Includes a simple combat loop between hardcoded units
- ✅ Clean, modular C# code for easy reuse or extension

---

## 📁 Core Files

| File              | Description                                                         |
|-------------------|---------------------------------------------------------------------|
| `MainClass.cs`    | Entry point and main simulation loop (loads map, runs combat logic) |
| `GameFunctions.cs`| Core Dijkstra algorithm for computing reachable tiles               |
| `Unit.cs`         | Defines unit stats and combat behavior                              |
| `Tile.cs`         | Represents the map and terrain system                               |

---

## 🧍 Unit Initialization

Units are currently initialized manually inside `MainClass.cs` using a single method `GameTest()` which defines both player and enemy units:

```csharp
List<Unit> playerUnits = new List<Unit>();
playerUnits.Add(playerUnit1);
...

List<Unit> enemyUnits = new List<Unit>();
enemyUnits.Add(enemyUnit1);
enemyUnits.Add(enemyUnit2);
...

Game game = new Game(board, tiles, playerUnits, enemyUnits);
game.PlayGame();
```

All units are instantiated and added within this method. To change unit types, stats, or positions, modify the definitions in `MainClass.cs`.

> ⚠️ Units are currently **not** loaded from JSON.

---

## 🔗 Integration with Level Creator

This simulation consumes map files exported from the [Strategy Game Level Creator](https://github.com/QuirkyQubits/strategy-game-tool).

To use it:

1. Open the web-based level creator
2. Design your level
3. Export the file (e.g. `strategy-game-export.json`)
4. Place the exported JSON in the project root

📍 **Note:** The simulation expects the file to be named:
```
strategy-game-export.json
```
If your file has a different name, either rename it or update the path in `MainClass.cs`.

---

## 🚀 How to Run

### Prerequisites

- ✅ [.NET 6+ SDK](https://dotnet.microsoft.com/en-us/download)
- ✅ Any C#-compatible IDE (e.g., Visual Studio, JetBrains Rider, VS Code)

---

### Run via Command Line

```bash
git clone https://github.com/QuirkyQubits/strategy-game-dijkstra-algorithm.git
cd strategy-game-dijkstra-algorithm
```

Place your exported JSON file (`strategy-game-export.json`) into the project root, then run:

```bash
dotnet run --project StrategyGameDjikstraAlgo
```

---

### Run via Visual Studio

1. Open the solution in Visual Studio
2. Ensure `strategy-game-export.json` is present in the project directory
3. Press ▶️ Run

---

## 🚧 Future Improvements

- [ ] Load unit data from JSON
- [ ] Add unit teams and AI behaviors
- [ ] Rebuild this backend in a game engine to make it a lot prettier!
- [ ] Extend to multi-turn simulations

---

## 📄 License

MIT License
