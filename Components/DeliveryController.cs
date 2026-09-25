using System;

namespace TGHE_Delivery
{
    internal sealed class DeliveryController
    {
        private readonly DeliveryInstance deliveryInstance;

        public DeliveryController(DeliveryInstance deliveryInstance)
        {
            this.deliveryInstance = deliveryInstance ?? throw new ArgumentNullException(nameof(deliveryInstance));
        }

        public long[] SolveAll()
        {
            long infiniteCapacity = ComputeInfiniteCapacity();

            var answersByLocation = new long[deliveryInstance.Locations.Count];
            for (int locationIndex = 0; locationIndex < deliveryInstance.Locations.Count; locationIndex++)
            {
                Location location = deliveryInstance.Locations[locationIndex];
                answersByLocation[locationIndex] = SolveLocation(location, infiniteCapacity);
            }

            return answersByLocation;
        }

        private long ComputeInfiniteCapacity()
        {
            long sumOfPlantPowers = 0;
            foreach (var plant in deliveryInstance.Plants)
            {
                if (plant.Power < 0)
                    throw new InvalidOperationException($"Plant power cant be negative (plant id={plant.Id}).");
                sumOfPlantPowers += plant.Power;
            }
            return sumOfPlantPowers;
        }

        private long SolveLocation(Location location, long infiniteCapacity)
        {
            int connectionPointCount = deliveryInstance.NodeCount;

            int sourceNode = connectionPointCount;
            int sinkNode = connectionPointCount + 1;

            var maxFlowSolver = new Dinic(nodeCount: connectionPointCount + 2);

            AddPlantEdges(maxFlowSolver, sourceNode, connectionPointCount);
            AddWireEdges(maxFlowSolver, connectionPointCount);
            AddLocationAttachmentEdges(maxFlowSolver, location, sinkNode, connectionPointCount, infiniteCapacity);

            return maxFlowSolver.MaxFlow(source: sourceNode, sink: sinkNode);
        }

        private void AddPlantEdges(Dinic maxFlowSolver, int sourceNode, int connectionPointCount)
        {
            foreach (var plant in deliveryInstance.Plants)
            {
                ValidateNodeIndex(plant.Id, connectionPointCount, $"Plant id {plant.Id} out of range.");
                maxFlowSolver.AddEdge(from: sourceNode, to: plant.Id, capacity: (long)plant.Power);
            }
        }

        private void AddWireEdges(Dinic maxFlowSolver, int connectionPointCount)
        {
            foreach (var wire in deliveryInstance.Wires)
            {
                ValidateNodeIndex(wire.A, connectionPointCount, $"Wire endpoint A {wire.A} out of range.");
                ValidateNodeIndex(wire.B, connectionPointCount, $"Wire endpoint B {wire.B} out of range.");

                if (wire.Capacity < 0)
                    throw new InvalidOperationException($"Wire capacity cant be negative (A={wire.A}, B={wire.B}).");

                maxFlowSolver.AddEdge(from: wire.A, to: wire.B, capacity: wire.Capacity);
                maxFlowSolver.AddEdge(from: wire.B, to: wire.A, capacity: wire.Capacity);
            }
        }

        private void AddLocationAttachmentEdges(
            Dinic maxFlowSolver,
            Location location,
            int sinkNode,
            int connectionPointCount,
            long infiniteCapacity)
        {
            foreach (int attachmentNode in location.Attachments)
            {
                ValidateNodeIndex(attachmentNode, connectionPointCount, $"Location attachment {attachmentNode} out of range.");
                maxFlowSolver.AddEdge(from: attachmentNode, to: sinkNode, capacity: infiniteCapacity);
            }
        }

        private static void ValidateNodeIndex(int nodeIndex, int nodeCount, string messageIfInvalid)
        {
            if ((uint)nodeIndex >= (uint)nodeCount)
                throw new InvalidOperationException(messageIfInvalid);
        }
    }
}