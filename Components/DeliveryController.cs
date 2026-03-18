using System;

namespace TGHE_Delivery
{
    internal sealed class DeliveryController
    {
        private readonly DeliveryInstance _ins;

        public DeliveryController(DeliveryInstance ins)
        {
            _ins = ins;
        }

        public int[] SolveAll()
        {
            // TODO: per-lokalita build Dinic + maxflow
            throw new NotImplementedException();
        }
    }
}