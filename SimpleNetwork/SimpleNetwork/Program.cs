
using SimpleNetwork;

Console.WriteLine("Program Started.");

#region XOR Gate Logic
List<Simulant> trainingData = new List<Simulant>();

// (0, 0) => 0
trainingData.Add(new Simulant()
{
    Inputs = new List<double>() { 0, 0 },
    WantedOutputs = new List<double> () { 0 },
});

// (0, 1) => 1
trainingData.Add(new Simulant()
{
    Inputs = new List<double>() { 0, 1 },
    WantedOutputs = new List<double>() { 1 },
});

// (1, 0) => 1
trainingData.Add(new Simulant()
{
    Inputs = new List<double>() { 1, 0 },
    WantedOutputs = new List<double>() { 1 },
});

// (1, 1) => 0
trainingData.Add(new Simulant()
{
    Inputs = new List<double>() { 1, 1 },
    WantedOutputs = new List<double>() { 0 },
});

#endregion

#region Generate Net

var network = new Net(2, 1, 2);

network.Train(trainingData, 0.05);

#endregion

#region Test and Display Results
Console.WriteLine("Commencing testing...");

foreach(var data in trainingData)
{
    var outputs = network.GetResults(data.Inputs);
    Console.WriteLine($"Input ({data.Inputs[0]},{data.Inputs[1]}) => Output ({outputs[0]})");
    Console.WriteLine($"Expected: ({data.WantedOutputs[0]})");
}

Console.WriteLine("Finished.");
Console.ReadLine();
#endregion