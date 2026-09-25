# TGHE Delivery

C# implementation of a graph algorithm for calculating the maximum electrical power that can be delivered through a distribution network.

## Assignment

The electrical network consists of:

- power plants with limited production capacity,
- distribution nodes,
- power lines with limited transmission capacity,
- candidate locations connected to up to three points in the network.

The goal is to calculate the **maximum available electrical power** for each candidate location.

The problem is modeled as a **maximum flow problem** in a capacitated graph.

## Solution

The project uses **Dinic's algorithm** to calculate the maximum flow through the network.

Power plant outputs and transmission limits are represented as edge capacities in the graph. For each requested location, the algorithm determines the maximum amount of power that can be transferred through the network without exceeding these limits.

### Complexity

Dinic's algorithm has a general worst-case time complexity of:

```text
O(V²E)
```

where:

- `V` is the number of vertices,
- `E` is the number of edges.

## Technologies

- C#
- .NET 8
- Graph algorithms
- Maximum flow
- Dinic's algorithm
- BFS / DFS

## Project Structure

```text
TGHE-Delivery/
├── Components/
├── Records/
├── Solver/
├── Properties/
├── program.cs
└── TGHE-Delivery.csproj
```

## Running the Project

Build:

```bash
dotnet build
```

Run:

```bash
dotnet run
```

The program reads input from standard input and writes the calculated maximum power values to standard output.

Example:

```bash
dotnet run < input.txt
```

## Example

Input:

```text
2 3 4
4
4
0 4 6
1 4 5
4 2 4
4 3 7
3
2
3
2 3
```

Output:

```text
4
7
8
```
