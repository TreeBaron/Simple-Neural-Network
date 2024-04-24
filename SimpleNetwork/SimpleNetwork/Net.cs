using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNetwork
{
    public class Net
    {
        public Net(int inputSize, int outputSize, int depth)
        {
            // Input layer
            var layer1 = new Layer(inputSize+2);
            Layers.Add(layer1);

            // Middle layers
            for(int i = 0; i < depth; i++) 
            {
                var middleLayer = new Layer(3);
                Layers.Add(middleLayer);
            }

            // Output layer
            var outputLayer = new Layer(outputSize);
            Layers.Add(outputLayer);

            // Link layers...
            for (int i = 1; i < Layers.Count; i++)
            {
                var lastLayer = Layers[i - 1];
                var nextLayer = Layers[i];
                lastLayer.LinkLayer(nextLayer);
            }
        }

        public List<Layer> Layers { get; set; } = new List<Layer>();

        public List<double> GetResults(List<double> inputs)
        {
            // Load inputs first
            var firstLayer = Layers[0];
            var localInputs = new List<double>(inputs);
            localInputs.Add(-1);
            localInputs.Add(1);
            for(int i = 0; i < localInputs.Count; i++)
            {
                firstLayer.Nodes[i].Result = localInputs[i];
            }

            for(int i = 0; i < Layers.Count; i++)
            {
                var layer = Layers[i];
                layer.CalcLayer();
            }

            var lastLayer = Layers.Last();

            var results = lastLayer.Nodes.Select(x => x.Result).ToList();

            // Clear all results
            foreach(var layer in Layers)
            {
                foreach(var node in layer.Nodes)
                {
                    node.Result = 0;
                }
            }

            return results;
        }

        public void Train(List<Simulant> trainingData, double acceptableScore)
        {
            Random r = new Random(17);
            while (true)
            {
                var layers = Layers[0..];
                foreach (var layer in layers)
                {
                    foreach (var node in layer.Nodes)
                    {
                        foreach (var key in node.InputNodes.Keys)
                        {
                            var originalScore = GetScore(trainingData);
                            var originalValue = node.InputNodes[key];
                            node.InputNodes[key] += r.NextDouble() < 0.5 ? -1* r.NextDouble() * 10 : r.NextDouble()* 10;
                            var newScore = GetScore(trainingData);

                            // Less is a better score!
                            if(newScore < originalScore)
                            {
                                Console.WriteLine("Improved node!");
                            }
                            else
                            {
                                // Set value back to original value
                                node.InputNodes[key] = originalValue;
                            }
                        }
                    }
                }

                var score = GetScore(trainingData);
                Console.WriteLine($"Scored: {score}");

                if(score < acceptableScore)
                {
                    Console.WriteLine("Training passed!");
                    return;
                }
            }
        }

        public double GetScore(List<Simulant> trainingData)
        {
            var score = 0.0;
            foreach(var simulant in trainingData)
            {
                var results = this.GetResults(simulant.Inputs);
                for (int i = 0; i < results.Count; i++)
                {
                    var wantedOutput = simulant.WantedOutputs[i];
                    var actualOutput = results[i];
                    score += Math.Abs(wantedOutput - actualOutput);
                }
            }
            return score;
        }
    }
}
