# 🧠 Strategy Game Simulation Engine (Dijkstra Reference)

This project is a minimal, console-based simulation that demonstrates how to implement **grid-based movement and combat logic** for a strategy game. It is designed as a **reference consumer** of exported JSON maps from the [Strategy Game Level Creator](https://github.com/QuirkyQubits/strategy-game-tool), showcasing how to:

- Parse terrain-based map data from JSON
- Apply a shortest-path algorithm (Dijkstra's) to compute valid movement tiles
- Simulate simple turn-based combat between units

---

## 🌟 Purpose

This simulation was built as a **proof of concept** to validate the JSON export format from the frontend level editor and to show how movement logic could be used inside a real game engine (e.g., Unity or Unreal).

Rather than serving as a full-featured engine, this project demonstrates:

- How terrain-aware movement is computed
- How units can be placed and moved on a grid
- How turn-based combat mechanics could be layered on top

---

## 🔍 Key Features

- ✅ Reads a tile grid from JSON (terrain type + movement cost)
- ✅ Implements Dijkstra's algorithm to calculate reachable tiles
- ✅ Includes a combat loop between hardcoded units
- ✅ Designed to be easily portable or translated into other languages

---

## 📁 Core Files

| File | Description |
|------|-------------|
| `GameFunctions.cs` | Contains the core Dijkstra algorithm for computing valid movement tiles |
| `Program.cs` | Main simulation loop - hardcodes units and drives the battle |
| `Unit.cs` | Defines unit stats and behavior |
| `Tile.cs` / `Grid.cs` | Represent the map and terrain system |

---

## 🤖 How Units Are Initialized

This project currently **does not parse units from JSON**. Instead, units are manually hardcoded in `Program.cs` for simplicity:

```csharp
Unit knight = new Unit("Knight", 3, 10, 1, 1);
Unit goblin = new Unit("Goblin", 2, 6, 1, 1);

grid.AddUnit(knight, 0, 0);
grid.AddUnit(goblin, 4, 4);
```

Please note that if you wish to modify the initial placement of units, you currently have to do it in the code.

---

## 🔗 Integration with Level Creator

This simulation consumes map files exported from the [Strategy Game Level Creator](https://github.com/QuirkyQubits/strategy-game-tool), a browser-based level editor. You can:

1. Draw a level in the editor
2. Export the JSON
3. Place the file in this repo and load it at runtime

---

## 🚀 How to Run

### Prerequisites

- ✅ [.NET SDK (C#)](https://dotnet.microsoft.com/en-us/download) installed (version 6.0 or later recommended)
- ✅ Visual Studio or any C#-compatible IDE (optional)

---

### Run via CLI

1. Clone this repo  
   ```bash
   git clone https://github.com/QuirkyQubits/strategy-game-dijkstra-algorithm.git
   cd strategy-game-dijkstra-algorithm
   ```

2. Place a valid `map.json` file (exported from the [Level Creator](https://github.com/QuirkyQubits/strategy-game-tool)) into the root of the project folder.

3. Run the simulation:  
   ```bash
   dotnet run
   ```

---

### Or Run via Visual Studio

1. Open the solution file in Visual Studio  
2. Ensure your exported `map.json` is present in the working directory  
3. Press ▶️ Run

---

## 💪 Future Improvements

- [ ] Load unit data from JSON
- [ ] Add unit teams and AI behaviors
- [ ] Rebuild this backend in a game engine to make it a lot prettier!
- [ ] Extend to multi-turn simulations

---

## 📄 License

MIT License
