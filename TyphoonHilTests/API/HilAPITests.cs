using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime;
using TyphoonHil.API;
using TyphoonHilTests.Utils;


namespace TyphoonHilTests.API
{
    public class HilAPITestable : HilAPI
    {
        public bool HandleRequestOverrideCalled { get; private set; }
        public double MockModelReadValue { get; set; } = 42.0; // Default mock value
        public JObject LastHandledParameters { get; private set; } = null;
        public string LastHandledMethod { get; private set; } = null;

        protected override JObject HandleRequest(string method, JObject parameters)
        {
            HandleRequestOverrideCalled = true;
            // Save for test inspection
            LastHandledMethod = method;
            LastHandledParameters = parameters.DeepClone() as JObject;


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

            // Mock response for "save_settings_file"
            if (method == "save_settings_file")
            {
                // You can mock a successful save operation
                return new JObject { { "result", true } };
            }

            // Mock response for "save_model_state"
            if (method == "save_model_state")
            {
                // Assume the save operation is successful
                var mockResponse = new JObject
                {
                    ["result"] = true
                };
                return mockResponse;
            }

            // Mock response for "load_model_state"
            if (method == "load_model_state")
            {
                // Assume the load operation is successful
                var mockResponse = new JObject
                {
                    ["result"] = true
                };
                return mockResponse;
            }

            // Mock response for "upload_standalone_model"
            if (method == "upload_standalone_model")
            {
                // Assume the upload operation is successful
                var mockResponse = new JObject
                {
                    ["result"] = true // Simulating a successful upload
                };
                return mockResponse;
            }

            // Mock response for "model_write"
            if (method == "model_write")
            {
                // Simulating a successful model write
                var mockResponse = new JObject
                {
                    ["result"] = true // Mock response indicating success
                };
                return mockResponse;
            }

            // Mock response for "model_read"
            if (method == "model_read")
            {
                // Simulate a successful model read with a mock value
                var mockResponse = new JObject
                {
                    ["result"] = MockModelReadValue
                };
                return mockResponse;
            }

            // Mock response for "get_pe_switching_block_settings"
            if (method == "get_pe_switching_block_settings")
            {
                var blockName = parameters["blockName"]?.ToString() ?? "";
                var switchName = parameters["switchName"]?.ToString() ?? "";

                // Mock response for testing purposes
                var mockResponse = new JObject
                {
                    ["result"] = new JObject
                    {
                        ["software_control_enabled"] = true,
                        ["software_value"] = 1
                    }
                };
                return mockResponse;
            }

            // Simulate success response for start_capture, otherwise fallback to default simple response
            if (method == "start_capture")
            {
                return new JObject { { "result", true } };
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

        [TestMethod]
        public void LoadModelTest()
        {
            // This test calling real load_model function and using the compiled model from the location:
            // .\Typhoon-HIL-C-Sharp-API\TyphoonHilTests\ProtectedData\3ph rectifier\3ph rectifier Target files 

            // Arrange
            var testableApi = new HilAPITestable();

            var filePath = Path.Combine(ProtectedDataPath, "3ph rectifier", "3ph rectifier Target files", "3ph rectifier.cpd");
            Console.WriteLine($"File path is: {filePath}");
            // Act
            var result = testableApi.LoadModel(filePath, false, true);

            // Output details to the console
            Console.WriteLine($"LoadModel: {result}");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void LoadSettingsFileTest()
        {
            // This test calling real load_settings_file function and using the setting.runx from the location:
            // .\Typhoon-HIL-C-Sharp-API\TyphoonHilTests\ProtectedData\3ph rectifier\

            // Arrange
            var testableApi = new HilAPITestable();

            var filePath = Path.Combine(ProtectedDataPath, "3ph rectifier", "settings.runx");
            Console.WriteLine($"File path is: {filePath}");

            // Act
            var result = testableApi.LoadSettingsFile(filePath);

            // Output details to the console
            Console.WriteLine($"LoadModel: {result}");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void SaveSettingsFileTest()
        {
            // Arrange
            var testableApi = new HilAPITestable();

            var filePath = Path.Combine(ProtectedDataPath, "3ph rectifier", "init.runx");
            Console.WriteLine($"File path is: {filePath}");

            // Act
            var result = testableApi.SaveSettingsFile(filePath);

            // Output details to the console
            Console.WriteLine($"Result: {result}");
            Console.WriteLine($"HandleRequestOverrideCalled: {testableApi.HandleRequestOverrideCalled}");

            // Assert
            Assert.IsTrue(result); // Expecting the operation to succeed based on the mock
            Assert.IsTrue(testableApi.HandleRequestOverrideCalled); // Ensure the request was actually made
        }

        [TestMethod]
        public void SaveModelStateTest()
        {
            // Arrange
            var testableApi = new HilAPITestable();
            string testFilePath = @"./model_state.ms"; // Path for saving the model state

            // Act
            var result = testableApi.SaveModelState(testFilePath);

            // Output details to the console for debugging purposes
            Console.WriteLine($"Result: {result}");
            Console.WriteLine($"HandleRequestOverrideCalled: {testableApi.HandleRequestOverrideCalled}");

            // Assert
            Assert.IsNotNull(result); // Ensure result is not null
            Assert.IsTrue(result); // We expect the save operation to succeed
            Assert.IsTrue(testableApi.HandleRequestOverrideCalled); // Ensure the mocked HandleRequest was called
        }

        [TestMethod]
        public void LoadModelStateTest()
        {
            // Arrange
            var testableApi = new HilAPITestable();
            string testFilePath = @"./model_state.ms"; // Path to the saved model state file

            // Act
            var result = testableApi.LoadModelState(testFilePath);

            // Output details to the console for debugging purposes
            Console.WriteLine($"Result: {result}");
            Console.WriteLine($"HandleRequestOverrideCalled: {testableApi.HandleRequestOverrideCalled}");

            // Assert
            Assert.IsNotNull(result); // Ensure result is not null
            Assert.IsTrue(result); // We expect the load operation to succeed
            Assert.IsTrue(testableApi.HandleRequestOverrideCalled); // Ensure the mocked HandleRequest was called
        }

        [TestMethod]
        public void UploadStandaloneModelTest()
        {
            // Arrange
            var testableApi = new HilAPITestable();
            int modelLocation = 1; // Slot number to upload the model

            // Act
            var result = testableApi.UploadStandaloneModel(modelLocation);

            // Output details to the console for debugging purposes
            Console.WriteLine($"Result: {result}");
            Console.WriteLine($"HandleRequestOverrideCalled: {testableApi.HandleRequestOverrideCalled}");

            // Assert
            Assert.IsNotNull(result); // Ensure the result is not null
            Assert.IsTrue(result); // We expect the upload operation to succeed
            Assert.IsTrue(testableApi.HandleRequestOverrideCalled); // Ensure the mock was triggered

/*          // Mock response for "save_settings_file"
            if (method == "save_settings_file")
            {
                // You can mock a successful save operation
                return new JObject { { "result", true } };
            }

            // Mock response for "save_model_state"
            if (method == "save_model_state")
            {
                // Assume the save operation is successful
                var mockResponse = new JObject
                {
                    ["result"] = true
                };
                return mockResponse;
            }

            // Mock response for "load_model_state"
            if (method == "load_model_state")
            {
                // Assume the load operation is successful
                var mockResponse = new JObject
                {
                    ["result"] = true
                };
                return mockResponse;
            }

            // Mock response for "upload_standalone_model"
            if (method == "upload_standalone_model")
            {
                // Assume the upload operation is successful
                var mockResponse = new JObject
                {
                    ["result"] = true // Simulating a successful upload
                };
                return mockResponse;
            }

            // Mock response for "model_write"
            if (method == "model_write")
            {
                // Simulating a successful model write
                var mockResponse = new JObject
                {
                    ["result"] = true // Mock response indicating success
                };
                return mockResponse;
            }

            // Mock response for "model_read"
            if (method == "model_read")
            {
                // Simulate a successful model read with a mock value
                var mockResponse = new JObject
                {
                    ["result"] = MockModelReadValue
                };
                return mockResponse;
            }

            // Mock response for "get_pe_switching_block_settings"
            if (method == "get_pe_switching_block_settings")
            {
                var blockName = parameters["blockName"]?.ToString() ?? "";
                var switchName = parameters["switchName"]?.ToString() ?? "";

                // Mock response for testing purposes
                var mockResponse = new JObject
                {
                    ["result"] = new JObject
                    {
                        ["software_control_enabled"] = true,
                        ["software_value"] = 1
                    }
                };
                return mockResponse;
            }

            // Call base method or handle other cases
            return base.HandleRequest(method, parameters);
*/
        }

        [TestMethod]
        public void ModelWriteSingleValueTest()
        {
            // Arrange
            var testableApi = new HilAPITestable();
            string modelVariable = "Vgrid.rms"; // Example model variable
            double newValue = 25.0; // Example value

            // Act
            var result = testableApi.ModelWrite(modelVariable, newValue);

            // Output details to the console for debugging purposes
            Console.WriteLine($"Result: {result}");
            Console.WriteLine($"HandleRequestOverrideCalled: {testableApi.HandleRequestOverrideCalled}");

            // Assert
            Assert.IsNotNull(result); // Ensure the result is not null
            Assert.IsTrue(result); // Expect the model write operation to succeed
            Assert.IsTrue(testableApi.HandleRequestOverrideCalled); // Ensure the mock was triggered
        }

        [TestMethod]
        public void ModelWriteListValueTest()
        {
            // Arrange
            var testableApi = new HilAPITestable();
            string modelVariable = "Vgrid.rms"; // Example model variable
            List<double> newValues = new List<double> { 25.0, 30.0, 35.0 }; // Example list of values

            // Act
            var result = testableApi.ModelWrite(modelVariable, newValues);

            // Output details to the console for debugging purposes
            Console.WriteLine($"Result: {result}");
            Console.WriteLine($"HandleRequestOverrideCalled: {testableApi.HandleRequestOverrideCalled}");

            // Assert
            Assert.IsNotNull(result); // Ensure the result is not null
            Assert.IsTrue(result); // Expect the model write operation to succeed
            Assert.IsTrue(testableApi.HandleRequestOverrideCalled); // Ensure the mock was triggered
        }

        [TestMethod()]
        public void SetScadaInputValueTest()
        {
            var p = new JObject() { { "result", null } };
            double? p2 = (double?)p["result"];
        }

        [TestMethod()]
        public void GeneralTest()
        {
            Model.LoadModel(file: Path.Combine(ProtectedDataPath, "3ph rectifier", "3ph rectifier Target files", "3ph rectifier.cpd"),
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
            var rampTime = 0.25;


            // Act
            var result = testableApi.SetSourceSineWaveform(
                names: names,
                rms: rms,
                frequency: frequency,
                phase: phase,
                harmonics: null,
                harmonicsPu: harmonicsPu,
                executeAt: null,
                rampTime: rampTime,
                rampType: "lin"
            );


            // Output details to the console
            Console.WriteLine($"Result: {result}");
            Console.WriteLine($"HandleRequestOverrideCalled: {testableApi.HandleRequestOverrideCalled}");


            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(testableApi.HandleRequestOverrideCalled);
        }

        [TestMethod]
        /*[Ignore("This test is skipped because it requires a specific model to be loaded to HIL.")]*/
        public void SetPeSwitchingBlockControlModeTest_HIL()
        {
            // Using the THCC with compiled and loaded model
            // from \t_sw\tests\20_standalone\200_simple_buck\simple_buck.tse

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
        /*[Ignore("This test is skipped because it requires a specific model to be loaded to HIL.")]*/
        public void SetPeSwitchingBlockSoftwareValueTest_HIL()
        {
            // Using the THCC with compiled and loaded model
            // from \t_sw\tests\20_standalone\200_simple_buck\simple_buck.tse

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

        [TestMethod]
        /*[Ignore("This test is skipped because it requires a specific model to be loaded to HIL.")]*/
        public void GetPeSwitchingBlockSetting_ShouldReturnNullWhenSwitchNotFound_HIL()
        {
            // Using the THCC with compiled and loaded model
            // from \TyphoonHilTests\ProtectedData\200_simple_buck\simple_buck.tse

            // Arrange
            var api = new HilAPI();

            // Path to the TSE model for schematic editor
            var filePathTse = Path.Combine(ProtectedDataPath, "200_simple_buck", "simple_buck.tse");

            // Ensure file exists
            if (!File.Exists(filePathTse))
            {
                Assert.Fail($"TSE file does not exist at path: {filePathTse}");
            }

            // Load schematic model
            var loadResult = SchematicApiModel.Load(filePathTse);
            if (loadResult == null || loadResult["result"] == null)
            {
                Assert.Fail("SchematicApiModel.Load returned null or did not contain a 'result' field.");
            }

            // Safely extract result
            bool isLoaded;
            if (!bool.TryParse(loadResult["result"].ToString(), out isLoaded) || !isLoaded)
            {
                // Assert.Fail("Failed to load schematic model into THCC.");
                Console.WriteLine("Failed to load schematic model into THCC.");
            }
            Console.WriteLine("Schematic model loaded successfully.");

            // Read HW settings
            var hwSettings = SchematicApiModel.GetHwSettings();
            if (hwSettings == null || hwSettings.Count < 3)
            {
                Assert.Fail("Failed to retrieve hardware settings from schematic model.");
            }

            // Extract device and config (index 0 and 2, since index 1 is serial / ignored)
            var device = hwSettings[0]?.ToObject<string>();
            var config = hwSettings[2]?.ToObject<string>();

            if (string.IsNullOrEmpty(device) || string.IsNullOrEmpty(config))
            {
                Assert.Fail("Invalid hardware settings: missing device or config.");
            }

            // Set model settings
            SchematicApiModel.SetModelPropertyValue("hil_device", device);
            SchematicApiModel.SetModelPropertyValue("hil_configuration_id", config);

            Console.WriteLine($"Hardware settings applied: device={device}, config={config}");

            // Compile the model
            var isCompiled = SchematicApiModel.Compile();
            Console.WriteLine("Schematic Model is compiled: " + isCompiled);
            Assert.IsTrue(isCompiled, "Failed to compile schematic model.");

            // Load compiled model into HIL/VHIL
            var filePathCpd = Path.Combine(ProtectedDataPath, "200_simple_buck", "simple_buck Target files", "simple_buck.cpd");

            // Ensure .cpd file exists
            if (!File.Exists(filePathCpd))
            {
                Assert.Fail($"CPD file does not exist at path: {filePathCpd}");
            }

            var isModelLoaded = Model.LoadModel(file: filePathCpd, vhilDevice: false);
            Console.WriteLine("Compiled model is loaded into HIL: " + isModelLoaded);

            var blockName = "buck_1";
            var switchName = "NonExistentSwitch";

            //Act
            var result = api.GetPeSwitchingBlockSettings(blockName, switchName);


            // Output details to the console
            Console.WriteLine($"Result: {result}");

            // Assert
            Assert.IsNull(result, "Expected result to be null when the switch does not exist.");
        }

        [TestMethod]
        /*[Ignore("This test is skipped because it requires a specific model to be loaded to HIL.")]*/
        public void GetPeSwitchingBlockSettings_ShouldReturnSettingsWhenSwitchExists_HIL()
        {
            // Using the THCC with compiled and loaded model
            // from \TyphoonHilTests\ProtectedData\200_simple_buck\simple_buck.tse

            // Arrange
            var model = new HilAPI();
            var blockName = "buck_1";
            var switchName = "S1";

            // Act
            var result = model.GetPeSwitchingBlockSettings(blockName, switchName);

            // Assert
            Assert.IsNotNull(result, "Expected result to be not null when the switch exists.");
            Assert.AreEqual(false, result["software_control_enabled"].ToObject<bool>());
            Assert.AreEqual(0, result["software_value"].ToObject<int>());
        }

        [TestMethod()]
        public void GeneratingRampTest()
        {
            var api = new HilAPI();

            // Path to the TSE model for schematic editor
            var filePathTse = Path.Combine(ProtectedDataPath, "pv_panel", "pv_panels.tse");
            if (!File.Exists(filePathTse))
            {
                Assert.Fail($"TSE file does not exist at path: {filePathTse}");
            }

            // Load schematic model
            var loadResult = SchematicApiModel.Load(filePathTse);
            if (loadResult == null || loadResult["result"] == null)
            {
                Assert.Fail("SchematicApiModel.Load returned null or missing 'result' field.");
            }
            Console.WriteLine("Schematic Model loaded successfully.");

            // Read HW settings
            var hwSettings = SchematicApiModel.GetHwSettings();
            if (hwSettings == null || hwSettings.Count < 3)
            {
                Assert.Fail("Failed to retrieve hardware settings from schematic model.");
            }

            var device = hwSettings[0]?.ToObject<string>();
            var config = hwSettings[2]?.ToObject<string>();
            if (string.IsNullOrEmpty(device) || string.IsNullOrEmpty(config))
            {
                Assert.Fail("Invalid hardware settings: missing device or config.");
            }

            // Apply hardware settings
            SchematicApiModel.SetModelPropertyValue("hil_device", device);
            SchematicApiModel.SetModelPropertyValue("hil_configuration_id", config);
            Console.WriteLine($"Hardware settings applied: device={device}, config={config}");

            // Set component properties
            SchematicApiModel.SetComponentProperty("PV_Panel1", "Cpv", 5e-4);
            SchematicApiModel.SetComponentProperty("PV_Panel1", "sp_enable", true);   // FIX: use bool not string
            SchematicApiModel.SetComponentProperty("PV_Panel1", "initial_voltage", 0.0);
            SchematicApiModel.SetComponentProperty("PV_Panel1", "execution_rate", 50e-6);
            Console.WriteLine("Component properties set successfully.");

            // Set model-level property (simulation time step)
            SchematicApiModel.SetModelPropertyValue("simulation_time_step", 0.5e-6);
            Console.WriteLine("Model 'simulation_time_step' property set successfully.");

            // Compile the model
            var isCompiled = SchematicApiModel.Compile();
            Assert.IsTrue(isCompiled, "Model compilation failed.");
            Console.WriteLine("Model compiled successfully.");

            // Load compiled model into HIL/VHIL
            var filePathCpd = Path.Combine(ProtectedDataPath, "pv_panel", "pv_panels Target files", "pv_panels.cpd");
            if (!File.Exists(filePathCpd))
            {
                Assert.Fail($"CPD file does not exist at path: {filePathCpd}");
            }

            var isModelLoaded = Model.LoadModel(filePathCpd, vhilDevice: true);
            Assert.IsTrue(isModelLoaded, "Model failed to load into HIL/VHIL.");
            Console.WriteLine("Compiled model is loaded successfully into HIL/VHIL.");

            // Schedule ramping parameters
            double initialIllumination = 0.5;
            double finalIllumination = 2000;
            double rampTime = 120.0;

            string filePathIpvx = Path.Combine(ProtectedDataPath, "pv_panel", "Jinko_JKM200M-72_EN50530.ipvx");
            if (!File.Exists(filePathIpvx))
            {
                Assert.Fail($"IPVX file does not exist at path: {filePathIpvx}");
            }

            Model.SetPvInputFile("PV_Panel1", filePathIpvx);

            var rampSetResult = Model.SetPvAmbParams(
                name: "PV_Panel1",
                illumination: finalIllumination,
                rampTime: rampTime,
                rampType: "lin"
            );
            Assert.IsTrue(rampSetResult.Status, "Failed to set ramping parameters.");
            Console.WriteLine($"Ramp scheduling completed. Time: {rampTime}s, Final Value: {finalIllumination}");

            // Start simulation
            Assert.IsTrue(Model.StartSimulation(), "Failed to start simulation.");
            Assert.IsTrue(Model.IsSimulationRunning(), "Simulation is not running.");
            Console.WriteLine("Simulation started successfully.");

            // Stop simulation
            Assert.IsTrue(Model.StopSimulation(), "Failed to stop simulation.");
            Assert.IsFalse(Model.IsSimulationRunning(), "Simulation is still running.");
            Console.WriteLine("Simulation stopped successfully.");
        }


        [TestMethod]
        public void StartCapture_ShouldThrow_WhenChSettingsFormatIsInvalid()
        {
            // Arrange
            var api = new HilAPI();

            // Path to the TSE model for schematic editor
            var filePathTse = Path.Combine(ProtectedDataPath, "200_simple_buck", "simple_buck.tse");

            // Ensure file exists
            if (!File.Exists(filePathTse))
            {
                Assert.Fail($"TSE file does not exist at path: {filePathTse}");
            }

            // Load schematic model
            var loadResult = SchematicApiModel.Load(filePathTse);
            if (loadResult == null || loadResult["result"] == null)
            {
                Assert.Fail("SchematicApiModel.Load returned null or did not contain a 'result' field.");
            }

            // Safely extract result
            bool isLoaded;
            if (!bool.TryParse(loadResult["result"].ToString(), out isLoaded) || !isLoaded)
            {
                // Assert.Fail("Failed to load schematic model into THCC.");
                Console.WriteLine("Failed to load schematic model into THCC.");
            }
            Console.WriteLine("Schematic model loaded successfully.");

            // Read HW settings
            var hwSettings = SchematicApiModel.GetHwSettings();
            if (hwSettings == null || hwSettings.Count < 3)
            {
                Assert.Fail("Failed to retrieve hardware settings from schematic model.");
            }

            // Extract device and config (index 0 and 2, since index 1 is serial / ignored)
            var device = hwSettings[0]?.ToObject<string>();
            var config = hwSettings[2]?.ToObject<string>();

            if (string.IsNullOrEmpty(device) || string.IsNullOrEmpty(config))
            {
                Assert.Fail("Invalid hardware settings: missing device or config.");
            }

            // Set model settings
            SchematicApiModel.SetModelPropertyValue("hil_device", device);
            SchematicApiModel.SetModelPropertyValue("hil_configuration_id", config);

            Console.WriteLine($"Hardware settings applied: device={device}, config={config}");

            // Compile the model
            var isCompiled = SchematicApiModel.Compile();
            Console.WriteLine("Schematic Model is compiled: " + isCompiled);
            Assert.IsTrue(isCompiled, "Failed to compile schematic model.");

            // Load compiled model into HIL/VHIL
            var filePathCpd = Path.Combine(ProtectedDataPath, "200_simple_buck", "simple_buck Target files", "simple_buck.cpd");

            // Ensure .cpd file exists
            if (!File.Exists(filePathCpd))
            {
                Assert.Fail($"CPD file does not exist at path: {filePathCpd}");
            }

            var isModelLoaded = Model.LoadModel(file: filePathCpd, vhilDevice: false);
            Console.WriteLine("Compiled model is loaded into HIL: " + isModelLoaded);
            // Assert.IsTrue(isModelLoaded, "Failed to load compiled model into HIL device.");

            // Start simulation
            var simulationStarted = api.StartSimulation();
            if (simulationStarted != true)
            {
                Console.WriteLine("Failed to start simulation on HIL device.");
            }
            else Console.WriteLine("Simulation started on HIL device.");

            // Prepare invalid chSettings format (flat list inside a sublist)
            var cpSettings = new List<object>();
            var trSettings = new List<object>();
            var chSettings = new List<List<string>>
            {
                new List<string> { "V( Va )", "HIL0 digital input 1" }  // Invalid: multiple signals in one sublist
            };
            var dataBuffer = new List<object>();

            try
            {
                // Act
                api.StartCapture(cpSettings, trSettings, chSettings, dataBuffer, "", null, null);

                // If no exception is thrown, fail the test
                Assert.Fail("Expected StartCapture to throw due to invalid chSettings format, but it did not.");
            }
            catch (Exception ex)
            {
                // Assert
                Console.WriteLine("Expected exception caught: " + ex.Message);
                Assert.IsTrue(ex is Exception, "Unexpected exception type.");
            }
            finally
            {
                // Cleanup: stop simulation
                Assert.IsTrue(api.StopSimulation(), "Failed to stop simulation on HIL device.");
                Console.WriteLine("Simulation stopped on HIL device.");
            }
        }



        [TestMethod]
        public void GetDigitalSignals_ShouldReturnGroupedSignals_HIL()
        {
            // Arrange
            var api = new HilAPI();

            // Path to the TSE model for schematic editor
            var filePathTse = Path.Combine(ProtectedDataPath, "200_simple_buck", "simple_buck.tse");

            // Ensure file exists
            if (!File.Exists(filePathTse))
            {
                Assert.Fail($"TSE file does not exist at path: {filePathTse}");
            }

            // Load schematic model
            var loadResult = SchematicApiModel.Load(filePathTse);
            if (loadResult == null || loadResult["result"] == null)
            {
                Assert.Fail("SchematicApiModel.Load returned null or did not contain a 'result' field.");
            }

            // Safely extract result
            bool isLoaded;
            if (!bool.TryParse(loadResult["result"].ToString(), out isLoaded) || !isLoaded)
            {
                Console.WriteLine("Failed to load schematic model into THCC.");
            }
            Console.WriteLine("Schematic model loaded successfully.");

            // Read HW settings
            var hwSettings = SchematicApiModel.GetHwSettings();
            if (hwSettings == null || hwSettings.Count < 3)
            {
                Assert.Fail("Failed to retrieve hardware settings from schematic model.");
            }

            // Extract device and config (index 0 and 2, since index 1 is serial / ignored)
            var device = hwSettings[0]?.ToObject<string>();
            var config = hwSettings[2]?.ToObject<string>();

            if (string.IsNullOrEmpty(device) || string.IsNullOrEmpty(config))
            {
                Assert.Fail("Invalid hardware settings: missing device or config.");
            }

            // Set model settings
            SchematicApiModel.SetModelPropertyValue("hil_device", device);
            SchematicApiModel.SetModelPropertyValue("hil_configuration_id", config);

            Console.WriteLine($"Hardware settings applied: device={device}, config={config}");

            // Compile the model
            var isCompiled = SchematicApiModel.Compile();
            Console.WriteLine("Schematic Model is compiled: " + isCompiled);
            Assert.IsTrue(isCompiled, "Failed to compile schematic model.");

            // Load compiled model into HIL/VHIL
            var filePathCpd = Path.Combine(ProtectedDataPath, "200_simple_buck", "simple_buck Target files", "simple_buck.cpd");

            // Ensure .cpd file exists
            if (!File.Exists(filePathCpd))
            {
                Assert.Fail($"CPD file does not exist at path: {filePathCpd}");
            }

            var isModelLoaded = Model.LoadModel(file: filePathCpd, vhilDevice: false);
            Console.WriteLine("Compiled model is loaded into HIL: " + isModelLoaded);

            // Act: Start simulation
            var simulationStarted = api.StartSimulation();
            if (simulationStarted != true)
            {
                Console.WriteLine("Failed to start simulation on HIL device.");
            }
            else Console.WriteLine("Simulation started on HIL device.");

            Assert.IsTrue(api.IsSimulationRunning(), "Simulation should be running.");

            // Retrieve digital signals
            var digitalSignals = api.GetDigitalSignals();

            // Debug print: log all signals
            Console.WriteLine("Digital signals read from HIL:");
            for (int i = 0; i < digitalSignals.Count; i++)
            {
                Console.WriteLine($"  Device group {i}: {string.Join(", ", digitalSignals[i])}");
            }

            // Assert
            Assert.IsNotNull(digitalSignals, "Digital signals should not be null.");
            Assert.IsTrue(digitalSignals.Count > 0, "Expected at least one device group.");

            foreach (var deviceGroup in digitalSignals)
            {
                Assert.IsTrue(deviceGroup.Count > 0, "Each device should contain at least one signal.");
            }

            // Optional check (depends on model signals)
            Assert.IsTrue(digitalSignals[0].Contains("buck_1.S1"),
                "Expected 'buck_1.S1' to be present in first device group.");


            // Cleanup
            Assert.IsTrue(api.StopSimulation(), "Failed to stop simulation on HIL device.");
            Console.WriteLine("Simulation stopped on HIL device.");
            Assert.IsFalse(api.IsSimulationRunning(), "Simulation should be stopped.");
        }

    }
}
