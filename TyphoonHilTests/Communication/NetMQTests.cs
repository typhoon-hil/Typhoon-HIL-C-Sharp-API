using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System;
using TyphoonHil.API;
using System.IO;
using System.Reflection;
using TyphoonHilTests.Utils;
using System.Threading;

namespace TyphoonHilTests.Communication
{
    [TestClass]
    public class NetMQTests
    {

        public SchematicAPI SchematicApiModel { get; set; }
        public HilAPI Model { get; set; }
        public string StartupPath { get; set; }
        public string TestDataPath { get; set; }
        public string ProtectedDataPath { get; set; }

        [TestInitialize]
        public void Init()
        {
            Model = new HilAPI();
            SchematicApiModel = new SchematicAPI();
            StartupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            TestDataPath = Path.Combine(StartupPath, "TestData");
            ProtectedDataPath = Path.Combine(StartupPath, "ProtectedData");

            if (Directory.Exists(TestDataPath)) TestUtils.ClearDirectory(TestDataPath);
        }

        [TestMethod]
        public void GeneralTest()
        {
            var mdl = new SchematicAPI();
            mdl.CreateNewModel("test");
        }

        [TestMethod]
        public void MeasurementSpeedBenchmarkTest()
        {
            var api = new HilAPI();
                                    
            var isModelLoaded = api.LoadModel(file: Path.Combine(ProtectedDataPath, "3ph rectifier", "3ph rectifier Target files", "3ph rectifier.cpd"),
            vhilDevice: false);
            Console.WriteLine("Compiled model is loaded into HIL: " + isModelLoaded);

            // Act: Start simulation
            var simulationStarted = api.StartSimulation();
            if (simulationStarted != true)
            {
                Console.WriteLine("Failed to start simulation on HIL device.");
            }
            else Console.WriteLine("Simulation started on HIL device.");

            Assert.IsTrue(api.IsSimulationRunning(), "Simulation should be running.");
            Thread.Sleep(1000);

            const int iterations = 1000;
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < iterations; i++)
            {
                // Example: read one analog value
                var value = api.ReadAnalogSignal("Va1"); // use actual API call
            }

            sw.Stop();
            Console.WriteLine($"{iterations} queries took {sw.ElapsedMilliseconds} ms");

            // Cleanup
            Assert.IsTrue(api.StopSimulation(), "Failed to stop simulation on HIL device.");
            Console.WriteLine("Simulation stopped on HIL device.");
            Assert.IsFalse(api.IsSimulationRunning(), "Simulation should be stopped.");
        }

    }
}