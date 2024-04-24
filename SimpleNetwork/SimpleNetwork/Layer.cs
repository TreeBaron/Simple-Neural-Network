using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNetwork
{
    public class Layer
    {
        public Layer(int width)
        {
            for(int i = 0; i < width; i++) 
            {
                Nodes.Add(new Node((double input) =>
                {
                    // Mathematical sigmoid function
                    return ((10 / (1 + Math.Pow(Math.E, -1 * input)))-5);
                }));
            }
        }

        public List<Node> Nodes { get; set; } = new List<Node>();

        public void CalcLayer()
        {
            foreach(var node in Nodes)
            {
                node.CalcNode();
            }
        }

        public void LinkLayer(Layer lastLayer)
        {
            foreach (var lastNode in lastLayer.Nodes)
            {
                foreach (var node in Nodes)
                {
                    lastNode.AddInputNode(node);
                }
            }
        }
    }
}
