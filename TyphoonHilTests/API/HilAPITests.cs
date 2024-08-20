using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using TyphoonHil.API;
using TyphoonHilTests.Utils;

namespace TyphoonHilTests.API
{
    public class HilAPITestable : HilAPI
    {
        public bool HandleRequestOverrideCalled { get; private set; }

        protected override JObject HandleRequest(string method, JObject parameters)
        {
            HandleRequestOverrideCalled = true;

            // Mock response for "get_pv_mpp"
            if (method == "get_pv_mpp")
            {
                // The first element should be a boolean (e.g., true), 
                // The second element should be an array of doubles [MaxPowerCurrent, MaxPowerVoltage]
                var mockResponse = new JObject
                {
                    ["result"] = new JArray(true, new JArray(10.5, 20.3))
                };
                return mockResponse;
            }

            // Simulate the response for the "get_pvs" method.
            if (method == "get_pvs")
            {
                // Mock response with a nested list of PVs
                var mockResponse = new JObject
                {
                    ["result"] = new JArray(
                        new JArray("pv1_device0", "pv2_device0"),
                        new JArray("pv1_device1"),
                        new JArray("pv1_device2", "pv2_device2", "pv3_device2")
                    )
                };
                return mockResponse;
            }

            // Simulate the response for the "set_source_sine_waveform" method
            if (method == "set_source_sine_waveform" || 
                method == "set_pe_switching_block_control_mode" || 
                method == "set_pe_switching_block_software_value")
            {
                return new JObject { { "result", true } };
            }

            // Simulate the response for the "get_pe_switching_blocks" method
            if (method == "get_pe_switching_blocks")
            {
                // Mock response with a nested list of switching blocks
                var mockResponse = new JObject
                {
                    ["result"] = new JArray(
                        new JArray("block1_device0", "block2_device0"),
                        new JArray("block1_device1"),
                        new JArray("block1_device2", "block2_device2", "block3_device2")
                    )
                };
                return mockResponse;
            }

            // Mock response for "set_pv_amb_params"
            if (method == "set_pv_amb_params")
            {
                // The first element should be a boolean (e.g., true),
                // The second element should be an array of doubles [MaxPowerCurrent, MaxPowerVoltage]
                var mockResponse = new JObject
                {
                    ["result"] = new JArray(true, new JArray(8.5, 18.3)) // Mocked values
                };
                return mockResponse;
            }

            // Mock response for "get_hil_serial_number"
            if (method == "get_hil_serial_number")
            {
                var mockResponse = new JObject
                {
                    ["result"] = new JArray("00606-01-00150", "00404-00-00045")
                };
                return mockResponse;
            }

            // Call base method or handle other cases
            return base.HandleRequest(method, parameters);
        }
    }

    [TestClass()]
    public class HilAPITests
    {
        public HilAPITests() { }

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

        [TestMethod()]
        public void SetScadaInputValueTest()
        {
            var p = new JObject() { {"result", null }};
            double? p2 = (double?)p["result"];
        }

        [TestMethod()]
        public void GeneralTest()
        {
            Model.LoadModel(file:Path.Combine(ProtectedDataPath, "3ph rectifier", "3ph rectifier Target files", "3ph rectifier.cpd"),
            vhilDevice: true);

            Model.LoadSettingsFile(
                Path.Combine(ProtectedDataPath, "3ph rectifier", "settings.runx"));

            Model.SetAnalogOutput(5, "V( Va )", 150.00, 5.00);

            Model.SetDigitalOutput(1, "digital input 1", true, false, 0);

            Model.SetMachineConstantTorque("machine 1", 2.5);
            Model.SetMachineLinearTorque("machine 1", 5.0);
            Model.SetMachineSquareTorque("machine 1", 6.0);
            Model.SetMachineConstantTorqueType("machine 1");
            Model.SetMachineInitialAngle("machine 1", 3.14);
            Model.SetMachineInitialSpeed("machine 1", 100.0);
            Model.SetMachineIncEncoderOffset("machine 1", 3.14);
            Model.SetMachineSinEncoderOffset("machine 1", 1.57);

            var harmonics = new List<Harmonic>() { new Harmonic(2, 23, 2) };
            Model.PrepareSourceSineWaveform(new List<string> { "Vb" }, rms: new List<double>() { 220 }, 
                frequency: new List<double>() { 50 }, phase: new List<double>() { 120 }, harmonics: harmonics);

            Model.PrepareSourceConstantValue("Vdc", 200);

            Model.StartSimulation();
            Assert.IsTrue(Model.IsSimulationRunning());
            
            Model.StopSimulation();
            Assert.IsFalse(Model.IsSimulationRunning());

            //Model.EnableAoLimiting(1, -1.0, 1.0, 0);
            //Model.DisableAoLimiting(1, 1);
            //Model.EndScriptByUser();
        }


        [TestMethod]
        public void GetPvMppTest()
        {
            // Arrange
            var testableApi = new HilAPITestable();

            // Act
            var result = testableApi.GetPvMpp("test_pv_panel");

            // Output details to the console
            Console.WriteLine($"Status: {result.Status}");
            Console.WriteLine($"MaxPowerCurrent: {result.MaxPowerCurrent}");
            Console.WriteLine($"MaxPowerVoltage: {result.MaxPowerVoltage}");
            Console.WriteLine($"HandleRequestOverrideCalled: {testableApi.HandleRequestOverrideCalled}");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Status);
            Assert.AreEqual(10.5, result.MaxPowerCurrent);
            Assert.AreEqual(20.3, result.MaxPowerVoltage);
            Assert.IsTrue(testableApi.HandleRequestOverrideCalled);
        }

        [TestMethod]
        [Ignore("This test is skipped because it requires a specific HIL device.")]
        public void GetHilSerialNumberTestWithStringMock_HIL()
        {
            // This test should be modified to use mock data.
            // Currently is using real implementation and HIL device.

            // Arrange
            var model = new HilAPI();

            // Act
            var result = model.GetHilSerialNumber();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(21, result.Count); // Assuming the mock returns 21 serial numbers
            Assert.AreEqual("00404-00-00076", result[0]);
        }


        [TestMethod]
        public void GetHilSerialNumberTest()
        {
            // Arrange 
            var testableApi = new HilAPITestable();

            // Act
            var result = testableApi.GetHilSerialNumber();

            // Output details to the console
            foreach (var serial in result)
            {
                Console.WriteLine($"Serial number is: {serial}");
            }
            Console.WriteLine($"HandleRequestOverrideCalled: {testableApi.HandleRequestOverrideCalled}");

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("00606-01-00150", result[0]);
            Assert.AreEqual("00404-00-00045", result[1]);
            Assert.IsTrue(testableApi.HandleRequestOverrideCalled);
        }

        [TestMethod]
        public void GetPvsTestWithStub()
        {
            // Arrange
            var testableApi = new HilAPITestable();

            // Act
            var result = testableApi.GetPvs();

            // Output details to the console
            foreach (var pvsstub in result)
            {
                Console.WriteLine($"PVS List:");
                foreach (var element in pvsstub)
                {
                    Console.WriteLine($"{element}");
                }
            }
            Console.WriteLine($"HandleRequestOverrideCalled: {testableApi.HandleRequestOverrideCalled}");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count); // Should contain 3 sub-lists, one for each device

            // Assert the contents of each sub-list
            CollectionAssert.AreEqual(new List<string> { "pv1_device0", "pv2_device0" }, result[0]);
            CollectionAssert.AreEqual(new List<string> { "pv1_device1" }, result[1]);
            CollectionAssert.AreEqual(new List<string> { "pv1_device2", "pv2_device2", "pv3_device2" }, result[2]);
        }

        [TestMethod]
        public void SetSourceSineWaveformTest()
        {
            // Arrange
            var testableApi = new HilAPITestable();
            var names = new List<string> { "Grid.Vs1", "Grid.Vs1" };
            var rms = new List<double> { 12470 / Math.Pow(3, 0.5), 230 };
            var frequency = new List<double> { 60, 50 };
            var phase = new List<double> { 0, 120 };
            var harmonicsPu = new List<Harmonic>
            {
            new Harmonic(3, 0.1, 0),
            new Harmonic(5, 0.05, 90),
            new Harmonic(7, 0.03, 270)
            };


            // Act
            var result = testableApi.SetSourceSineWaveform(names, rms, frequency, phase, harmonicsPu);
            

            // Output details to the console
            Console.WriteLine($"Result: {result}");
            Console.WriteLine($"HandleRequestOverrideCalled: {testableApi.HandleRequestOverrideCalled}");


            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(testableApi.HandleRequestOverrideCalled);
        }

        [TestMethod]
        [Ignore("This test is skipped because it requires a specific model to be loaded to HIL.")]
        public void SetPeSwitchingBlockControlModeTest_HIL()
        {
            // Using the THCC with compiled and loaded model
            // from \t_sw\tests\20_standalone\200_simple_buck\hil_model\simple_buck.tse

            var model = new HilAPI();
            var blockName = "buck_1";
            var switchName = "S1";
            var swControl = true;
            var executeAt = 12345.678;

            var result = model.SetPeSwitchingBlockControlMode(blockName, switchName, swControl, executeAt);
            Console.WriteLine("SetPeSwitchingBlockControlModeTest - result: " + result.ToString());

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SetPeSwitchingBlockControlModeTest()
        {
            // Arrange
            var testableApi = new HilAPITestable();
            var blockName = "3ph_inverter 1";
            var switchName = "Sa_top";
            var swControl = true;
            var executeAt = 12345.678;

            // Act
            var result = testableApi.SetPeSwitchingBlockControlMode(blockName, switchName, swControl, executeAt);

            // Output details to the console
            Console.WriteLine($"Result: {result}");
            Console.WriteLine($"HandleRequestOverrideCalled: {testableApi.HandleRequestOverrideCalled}");

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(testableApi.HandleRequestOverrideCalled);
        }

        [TestMethod]
        [Ignore("This test is skipped because it requires a specific model to be loaded to HIL.")]
        public void SetPeSwitchingBlockSoftwareValueTest_HIL()
        {
            // Using the THCC with compiled and loaded model
            // from \t_sw\tests\20_standalone\200_simple_buck\hil_model\simple_buck.tse

            // Arange
            var model = new HilAPI();
            var blockName = "buck_1";
            var switchName = "S1";
            var value = 1;
            // var executeAt = 12345.678;

            //Act
            var result = model.SetPeSwitchingBlockSoftwareValue(blockName, switchName, value);


            // Output details to the console
            Console.WriteLine($"Result: {result}");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SetPeSwitchingBlockSoftwareValueTest()
        {
            // Arrange
            var testableApi = new HilAPITestable();
            var blockName = "buck_1";
            var switchName = "S1";
            var value = 1;
            var executeAt = 12345.678;

            // Act
            var result = testableApi.SetPeSwitchingBlockSoftwareValue(blockName, switchName, value, executeAt);

            // Output details to the console
            Console.WriteLine($"Result: {result}");
            Console.WriteLine($"HandleRequestOverrideCalled: {testableApi.HandleRequestOverrideCalled}");

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(testableApi.HandleRequestOverrideCalled);
        }

        [TestMethod]
        public void GetPeSwitchingBlocksTest()
        {
            // Arrange
            var testableApi = new HilAPITestable();

            // Act
            var result = testableApi.GetPeSwitchingBlocks();

            // Output details to the console
            foreach (var deviceBlocks in result)
            {
                Console.WriteLine("Device Blocks:");
                foreach (var block in deviceBlocks)
                {
                    Console.WriteLine($"  - {block}");
                }
            }
            Console.WriteLine($"The number of device lists: {result.Count}");
            Console.WriteLine($"HandleRequestOverrideCalled: {testableApi.HandleRequestOverrideCalled}");

            // Assert
            Assert.IsNotNull(result); // Ensure the result is not null
            Assert.AreEqual(3, result.Count); // Ensure there are three device lists

            // Check individual lists
            CollectionAssert.AreEqual(new List<string> { "block1_device0", "block2_device0" }, result[0]);
            CollectionAssert.AreEqual(new List<string> { "block1_device1" }, result[1]);
            CollectionAssert.AreEqual(new List<string> { "block1_device2", "block2_device2", "block3_device2" }, result[2]);
        }

        [TestMethod]
        public void SetPvAmbParamsTest()
        {
            // Arrange
            var testableApi = new HilAPITestable();
            string testName = "test_pv_panel";
            double testIllumination = 1000.0;
            double testTemperature = 25.0;
            double testIsc = 8.0;
            double testVoc = 37.0;
            double testExecuteAt = 0.0;
            double testRampTime = 5.0;
            string testRampType = "lin";

            // Act
            var result = testableApi.SetPvAmbParams(
                testName,
                illumination: testIllumination,
                temperature: testTemperature,
                isc: testIsc,
                voc: testVoc,
                executeAt: testExecuteAt,
                rampTime: testRampTime,
                rampType: testRampType
            );

            // Output details to the console
            Console.WriteLine($"Status: {result.Status}");
            Console.WriteLine($"MaxPowerCurrent: {result.MaxPowerCurrent}");
            Console.WriteLine($"MaxPowerVoltage: {result.MaxPowerVoltage}");

            Console.WriteLine($"HandleRequestOverrideCalled: {testableApi.HandleRequestOverrideCalled}");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Status);
            Assert.AreEqual(8.5, result.MaxPowerCurrent, 0.001); // Mocked expected value
            Assert.AreEqual(18.3, result.MaxPowerVoltage, 0.001); // Mocked expected value
            Assert.IsTrue(testableApi.HandleRequestOverrideCalled);
        }


    }
}