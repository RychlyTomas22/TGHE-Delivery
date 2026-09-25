using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGHE_Delivery
{
    internal abstract class Node
    {
        public int Id { get; }

        protected Node(int id)
        {
            Id = id;
        }
    }
}