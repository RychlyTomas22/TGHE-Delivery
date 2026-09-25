// Solver/Dinic.cs
using System;
using System.Collections.Generic;

namespace TGHE_Delivery
{
    internal sealed class Dinic
    {
        private sealed class Edge
        {
            public int To { get; }
            public int ReverseEdgeIndex { get; }
            public long ResidualCapacity { get; set; }

            public Edge(int to, int reverseEdgeIndex, long residualCapacity)
            {
                To = to;
                ReverseEdgeIndex = reverseEdgeIndex;
                ResidualCapacity = residualCapacity;
            }
        }

        private readonly List<Edge>[] adjacencyList;
        private readonly int[] levelByNode;
        private readonly int[] nextEdgeIndexToTry;

        public int NodeCount => adjacencyList.Length;

        public Dinic(int nodeCount)
        {
            if (nodeCount <= 0) throw new ArgumentOutOfRangeException(nameof(nodeCount));

            adjacencyList = new List<Edge>[nodeCount];
            for (int i = 0; i < nodeCount; i++)
            {
                adjacencyList[i] = new List<Edge>();
            }

            levelByNode = new int[nodeCount];
            nextEdgeIndexToTry = new int[nodeCount];
        }

        public void AddEdge(int from, int to, int capacity) => AddEdge(from, to, (long)capacity);

        public void AddEdge(int from, int to, long capacity)
        {
            if ((uint)from >= (uint)NodeCount) throw new ArgumentOutOfRangeException(nameof(from));
            if ((uint)to >= (uint)NodeCount) throw new ArgumentOutOfRangeException(nameof(to));
            if (capacity < 0) throw new ArgumentOutOfRangeException(nameof(capacity));

            int indexOfReverseInToList = adjacencyList[to].Count;
            int indexOfReverseInFromList = adjacencyList[from].Count;

            var forwardEdge = new Edge(to, indexOfReverseInToList, capacity);
            var reverseEdge = new Edge(from, indexOfReverseInFromList, 0);

            adjacencyList[from].Add(forwardEdge);
            adjacencyList[to].Add(reverseEdge);
        }

        public long MaxFlow(int source, int sink)
        {
            if ((uint)source >= (uint)NodeCount) throw new ArgumentOutOfRangeException(nameof(source));
            if ((uint)sink >= (uint)NodeCount) throw new ArgumentOutOfRangeException(nameof(sink));
            if (source == sink) return 0;

            long totalFlow = 0;

            while (BuildLevelGraph(source, sink))
            {
                Array.Fill(nextEdgeIndexToTry, 0);

                while (true)
                {
                    long pushed = SendBlockingFlow(source, sink, long.MaxValue / 4);
                    if (pushed == 0) break;
                    totalFlow += pushed;
                }
            }

            return totalFlow;
        }

        private bool BuildLevelGraph(int source, int sink)
        {
            Array.Fill(levelByNode, -1);

            var queue = new Queue<int>();
            levelByNode[source] = 0;
            queue.Enqueue(source);

            while (queue.Count > 0)
            {
                int current = queue.Dequeue();

                foreach (var edge in adjacencyList[current])
                {
                    if (edge.ResidualCapacity <= 0) continue;
                    if (levelByNode[edge.To] != -1) continue;

                    levelByNode[edge.To] = levelByNode[current] + 1;

                    if (edge.To == sink) return true;
                    queue.Enqueue(edge.To);
                }
            }

            return levelByNode[sink] != -1;
        }

        private long SendBlockingFlow(int currentNode, int sink, long flowLimit)
        {
            if (currentNode == sink) return flowLimit;

            for (int i = nextEdgeIndexToTry[currentNode];
                 i < adjacencyList[currentNode].Count;
                 i++, nextEdgeIndexToTry[currentNode]++)
            {
                var edge = adjacencyList[currentNode][i];

                if (edge.ResidualCapacity <= 0) continue;
                if (levelByNode[edge.To] != levelByNode[currentNode] + 1) continue;

                long pushed = SendBlockingFlow(edge.To, sink, Math.Min(flowLimit, edge.ResidualCapacity));
                if (pushed <= 0) continue;

                edge.ResidualCapacity -= pushed;
                adjacencyList[edge.To][edge.ReverseEdgeIndex].ResidualCapacity += pushed;

                return pushed;
            }

            return 0;
        }
    }
}