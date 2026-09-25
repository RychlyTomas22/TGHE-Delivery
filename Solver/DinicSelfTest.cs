using System;

namespace TGHE_Delivery
{
    internal static class DinicSelfTest
    {
        public static void Run()
        {
            // 0->1 (3), 0->2 (2), 1->2 (5), 1->3 (2), 2->3 (3)
            // MaxFlow(0,3) = 5
            var maxFlowSolver = new Dinic(nodeCount: 4);

            maxFlowSolver.AddEdge(from: 0, to: 1, capacity: 3);
            maxFlowSolver.AddEdge(from: 0, to: 2, capacity: 2);
            maxFlowSolver.AddEdge(from: 1, to: 2, capacity: 5);
            maxFlowSolver.AddEdge(from: 1, to: 3, capacity: 2);
            maxFlowSolver.AddEdge(from: 2, to: 3, capacity: 3);

            long computedFlow = maxFlowSolver.MaxFlow(source: 0, sink: 3);
            if (computedFlow != 5)
                throw new InvalidOperationException($"self-test failed: expected 5, got {computedFlow}.");

            Console.Error.WriteLine("self-test OK (maxflow=5).");
        }
    }
}