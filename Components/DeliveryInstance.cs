using System;
using System.Collections.Generic;

namespace TGHE_Delivery
{
    internal sealed class DeliveryInstance
    {
        public IReadOnlyList<Plant> Plants { get; init; } = Array.Empty<Plant>();
        public IReadOnlyList<Substation> Substations { get; init; } = Array.Empty<Substation>();
        public IReadOnlyList<Wire> Wires { get; init; } = Array.Empty<Wire>();
        public IReadOnlyList<Location> Locations { get; init; } = Array.Empty<Location>();

        public int NodeCount => Plants.Count + Substations.Count;
    }
}
