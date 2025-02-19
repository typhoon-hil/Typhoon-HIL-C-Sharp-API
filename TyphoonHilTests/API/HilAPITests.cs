using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using TyphoonHil.API;
using TyphoonHilTests.Utils;

/*
namespace TyphoonHil.API.Tests
{
    [TestClass()]
    public class HilAPITests
    {
        [TestMethod()]
        public void SaveModelStateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UploadStandaloneModelTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddDataLoggerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void HilAPITest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LoadModelTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LoadSettingsFileTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void StopDataLoggerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetSourceArbitraryWaveformTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetPeSwitchingBlockControlModeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetPeSwitchingBlockSoftwareValueTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetAnalogOutputTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetDigitalOutputTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetMachineConstantTorqueTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetMachineSquareTorqueTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetMachineLinearTorqueTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetMachineInitialAngleTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetMachineInitialSpeedTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetMachineIncEncoderOffsetTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetMachineSinEncoderOffsetTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void StartSimulationTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void StartCaptureTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CaptureInProgressTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void StopSimulationTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EndScriptByUserTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetMachineConstantTorqueTypeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LoadModelStateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ModelWriteTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ModelReadTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RemoveDataLoggerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void StartDataLoggerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateSourcesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PrepareSourceArbitraryWaveformTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PrepareSourceConstantValueTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PrepareSourceSineWaveformTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EnableAoLimitingTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DisableAoLimitingTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetBootConfigurationTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetSourceConstantValueTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetSourceSineWaveformTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetSourceScalingTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetPvInputFileTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetPvAmbParamsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetAnalogOutputSignalTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetAnalogOutputScalingTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetAnalogOutputOffsetTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetDigitalOutputSignalTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetDigitalOutputInvertingTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetDigitalOutputSwControlTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetDigitalOutputSoftwareValueTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetContactorTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetContactorControlModeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetContactorStateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetMachineLoadSourceTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetMachineExternalTorqueTypeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetMachineLoadTypeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetMachineSpeedTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetMachineEncoderOffsetTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetMachineResolverOffsetTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetInitialBatterySocTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetScadaInputValueTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetCpInputValueTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetTextModeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetDebugLevelTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void StopCaptureTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsSimulationRunningTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CheckHilHwidTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TimeoutOccurredTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReadPvIvCurveTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReadAnalogSignalTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReadAnalogSignalsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReadDigitalSignalTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReadDigitalSignalsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReadDigitalInputTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReadStreamingSignalsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LoadSignalGenDataTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateSignalStimulusTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PrepareSignalStimulusTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void StartSignalStimulusTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void StopSignalStimulusTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PauseSignalStimulusTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RebootHilTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void WaitSecTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void WaitMsecTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void WaitOnUserTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ResetFlagStatusTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetModelVariablesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetCpOutputValueTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetScadaOutputValueTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetBatterySocTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetPvMppTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetNumOfConnectedHilsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSimStepTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSimTimeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetDeviceCfgListTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSwVersionTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetHilCalibrationDateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetDeviceFeaturesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetHwInfoTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetFlagStatusTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSourcesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetPvsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAnalogSignalsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetDigitalSignalsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetStreamingAnalogSignalsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetStreamingDigitalSignalsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetContactorsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetMachinesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetPeSwitchingBlocksTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetScadaInputsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetScadaOutputsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSourceSettingsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetPvPanelSettingsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetMachineSettingsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetContactorSettingsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAnalogOutputSettingsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetDigitalOutputSettingsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetCpInputSettingsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetScadaInputSettingsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetHilSerialNumberTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetNsVarTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetNsVarsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetDataLoggerStatusTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetModelFilePathTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSpMonitorsValuesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AvailableSourcesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AvailablePvsTest()
        {
Assert.Fail();
        }
    }
}
*/

namespace TyphoonHilTests.API
{
    public class HilAPITestable : HilAPI
    {
        public bool HandleRequestOverrideCalled { get; private set; }
        public double MockModelReadValue { get; set; } = 42.0; // Default mock value

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
            _ = (double?)p["result"];
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
        [Ignore("This test is skipped because it requires a specific model to be loaded to HIL.")]
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
        [Ignore("This test is skipped because it requires a specific model to be loaded to HIL.")]
        public void GetPeSwitchingBlockSetting_ShouldReturnNullWhenSwitchNotFound_HIL()
        {
            // Using the THCC with compiled and loaded model
            // from \TyphoonHilTests\ProtectedData\200_simple_buck\simple_buck.tse

            // Arange
            var model = new HilAPI();
            var blockName = "buck_1";
            var switchName = "NonExistentSwitch";

            //Act
            var result = model.GetPeSwitchingBlockSettings(blockName, switchName);


            // Output details to the console
            Console.WriteLine($"Result: {result}");

            // Assert
            Assert.IsNull(result, "Expected result to be null when the switch does not exist.");
        }

        [TestMethod]
        [Ignore("This test is skipped because it requires a specific model to be loaded to HIL.")]
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
            var filePathTse = Path.Combine(ProtectedDataPath, "pv_panel", "pv_panels.tse");

            var isLoaded = SchematicApiModel.Load(filePathTse);
            Console.WriteLine("Schematic Model is loaded: " + isLoaded.ToString());
            // Assert.IsTrue(isLoaded, "Model failed to load.");

            // Set properties directly
            SchematicApiModel.SetComponentProperty("PV1", "Cpv", 5e-4);
            SchematicApiModel.SetComponentProperty("PV1", "sp_enable", true);
            SchematicApiModel.SetComponentProperty("PV1", "initial_voltage", 0);
            SchematicApiModel.SetComponentProperty("PV1", "execution_rate", 50e-6);

            Console.WriteLine("Component properties set successfully.");

            // Set model-level property (simulation time step)
            SchematicApiModel.SetModelPropertyValue("simulation_time_step", 0.5e-6);
            Console.WriteLine("Model 'simulation_time_step' property set successfully.");

            // Compile the model
            SchematicApiModel.Compile();
            // Assert.IsTrue(isCompiled, "Model compilation failed.");
            Console.WriteLine("Model compiled successfully.");

            // Load the model to the HIL/VHIL with the PV panel
            var isModelLoaded = Model.LoadModel(file: Path.Combine(ProtectedDataPath, "pv_panel", "pv_panels Target files", "pv_panels.cpd"),
            vhilDevice: true);
            Console.WriteLine("Model is loaded: " + isModelLoaded.ToString());
            Assert.IsTrue(isModelLoaded, "Model failed to load.");

            // Schedule ramping parameters
            double initialIllumination = 0.5; // Initial current value in Amps
            double finalIllumination = 4.5;   // Final current value in Amps
            double rampTime = 120.0;          // Ramp duration in seconds
            string filePathIpvx = Path.Combine(ProtectedDataPath, "pv_panel", "Jinko_JKM200M-72_EN50530.ipvx");

            Model.SetPvInputFile("PV1", filePathIpvx);

            var rampSetResult = Model.SetPvAmbParams(
                name: "PV1",
                illumination: finalIllumination,
                rampTime: rampTime,
                rampType: "lin"
            );
            Assert.IsTrue(rampSetResult.Status, "Failed to set ramping parameters.");

            Console.WriteLine($"Ramp scheduling completed. Time: {rampTime}s, Initial Value: {initialIllumination}, Final Value: {finalIllumination}");

            // Start the simulation
            Model.StartSimulation();
            Assert.IsTrue(Model.IsSimulationRunning(), "Simulation is not running.");
            Console.WriteLine("Simulation is started: " + Model.StartSimulation().ToString());

            Model.StopSimulation();
            Assert.IsFalse(Model.IsSimulationRunning());
            Console.WriteLine("Simulation is stopped: " + Model.StopSimulation().ToString());
        }

    }
}