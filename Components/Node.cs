using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGHE_Delivery.components
{
    abstract class Node
    {
        public int ID { get; }

        protected Node(int id)
        {
            ID = id;
        }
    }
}
