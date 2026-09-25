using System;

namespace TGHE_Delivery
{
    internal sealed class Plant : Node
    {
        public int Power { get; }

        public Plant(int id, int power) : base(id)
        {
            Power = power;
        }
    }
}