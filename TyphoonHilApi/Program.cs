﻿using Newtonsoft.Json.Linq;
using TyphoonHilApi.Communication.APIs;

namespace TyphoonHilApi
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a Typhoon HIL API instance
            var hil = new HilAPI();

            // Load the model
            hil.LoadModel(file: @"C:\Program Files\Typhoon HIL Control Center 2023.3\examples\models\general power electronics\3ph rectifier\3ph rectifier Target files\3ph rectifier.cpd", vhilDevice:true);

            // Load the settings file
            hil.LoadSettingsFile(file: @"C:\Program Files\Typhoon HIL Control Center 2023.3\examples\models\general power electronics\3ph rectifier\settings.runx");

            //// Set input waveforms
            hil.SetSourceArbitraryWaveform("Vgrid_a", file: @"C:\Program Files\Typhoon HIL Control Center 2023.3\examples\inputs\sources\110V_60Hz_phase_a.isg");
            hil.SetSourceArbitraryWaveform("Vb", file: @"C:\Program Files\Typhoon HIL Control Center 2023.3\examples\inputs\sources\110V_60Hz_phase_b.isg");
            hil.SetSourceArbitraryWaveform("Vc", file: @"C:\Program Files\Typhoon HIL Control Center 2023.3\examples\inputs\sources\110V_60Hz_phase_c.isg");

            // Set switching block control
            hil.SetPeSwitchingBlockControlMode(blockName: "3ph_inverter 1", switchName: "Sa_top", swControl: true);
            hil.SetPeSwitchingBlockSoftwareValue(blockName: "3ph_inverter 1", switchName: "Sa_top", value: 1);

            // Set analog output
            hil.SetAnalogOutput(5, "V( Va )", scaling: 150.00, offset: 5.00);

            // Set digital output
            hil.SetDigitalOutput(1, name: "digital input 1", invert: true, swControl: false, value: 0);

            // Set machine parameters
            hil.SetMachineConstantTorque(name: "machine 1", value: 2.5);
            hil.SetMachineLinearTorque(name: "machine 1", value: 5.0);
            hil.SetMachineSquareTorque(name: "machine 1", value: 6.0);
            hil.SetMachineConstantTorqueType(name: "machine 1", frictional: true);
            hil.SetMachineInitialAngle(name: "machine 1", angle: 3.14);
            hil.SetMachineInitialSpeed(name: "machine 1", speed: 100.0);
            hil.SetMachineIncEncoderOffset(name: "machine 1", offset: 3.14);
            hil.SetMachineSinEncoderOffset(name: "machine 1", offset: 1.57);

            // Start the simulation
            hil.StartSimulation();

            // Capture settings
            var captureSettings = new object[] { 1, 3, 1e5, true };
            var triggerSettings = new object[] { "Analog", 1, 0.0, "Rising edge", 50.0 };
            var channelSettings = new string[] { "V(Va)", "V(Vb)", "V(Vc)" };

            // Create a data buffer
            var capturedDataBuffer = new System.Collections.Generic.List<object>();

            // Start capture
            if (hil.StartCapture(captureSettings.ToList(), triggerSettings.ToList(), channelSettings.ToList(), dataBuffer: capturedDataBuffer, fileName: @"C:\Users\Dell\source\repos\TyphoonHilApi\TestData\capture_test.mat"))
            {
                // Wait for capture to finish
                while (hil.CaptureInProgress())
                {
                    // Waiting for capture to complete
                }

                // Unpack data from the data buffer
                var signalsNames = (string[])capturedDataBuffer[0];
                var yDataMatrix = (double[])capturedDataBuffer[1];
                var xData = (double[])capturedDataBuffer[2];

                // Unpack data for appropriate captured signals
                var VaData = yDataMatrix[0]; // Assuming you have a GetRow method for 2D arrays
                var VbData = yDataMatrix[1];
                var VcData = yDataMatrix[2];
            }
            else
            {
                // If an error occurred
                Console.WriteLine("Unable to start capture process.");
            }

            // Stop the simulation
            hil.StopSimulation();

            // End the script
            hil.EndScriptByUser();
        }

        private static void Test31()
        {
            string path = "C:\\Users\\Dell\\source\\repos\\TyphoonHilApi\\TyphoonHilApiTests\\TestData\\";
            var PvGenerator = new PvGeneratorAPI();
            var PvParamsDetailed = new JObject
            {
                { "Voc_ref", 45.60 },
                { "Isc_ref", 5.8 },
                { "dIsc_dT", 0.0004 },
                { "Nc", 72 },
                { "dV_dI_ref", -1.1 },
                { "Vg", "cSi" },
                { "n", 1.3 },
                { "neg_current", false }
            };

            var res = PvGenerator.GeneratePvSettingsFile(PvModelType.DETAILED, path + "setDet.ipvx", PvParamsDetailed);

            var PvParamsEN50530 = new JObject
            {
                { "Voc_ref", 45.60 },
                { "Isc_ref", 5.8 },
                { "Pv_type", "Thin film" },
                { "neg_current", false }
            };

            res = PvGenerator.GeneratePvSettingsFile(PvModelType.EN50530, path + "setEN.ipvx", PvParamsEN50530);

            var PvParamsUserDefined = new JObject
            {
                { "Voc_ref", 45.60 },
                { "Isc_ref", 5.8 },
                { "Pv_type", "User defined" },
                { "neg_current", false },
                {
                    "user_defined_params", new JObject
                    {
                        { "ff_u", 0.72 },
                        { "ff_i", 0.8 },
                        { "c_g", 1.252e-3 },
                        { "c_v", 8.419e-2 },
                        { "c_r", 1.476e-4 },
                        { "v_l2h", 0.98 },
                        { "alpha", 0.0002 },
                        { "beta", -0.002 }
                    }
                }
            };

            res = PvGenerator.GeneratePvSettingsFile(PvModelType.NORMALIZED_IV, path + "setIV.ipvx", PvParamsUserDefined);

            var PvParamsCSV = new JObject
            {
                { "csv_path", "csv_file.csv" }
            };

            res = PvGenerator.GeneratePvSettingsFile(PvModelType.EN50530, path + "./setEN.csv.ipvx", PvParamsCSV);
        }

        private static void Test30()
        {
            ConfigurationManagerAPI cm = new();

            string basePath = "C:\\Users\\Dell\\source\\repos\\TyphoonHilApi\\TestData\\drive example\\";
            string prj_file = basePath + "example_project.cmp";
            string cfgPath = basePath + "configs";
            string outPath = basePath + "output";

            JObject prj = cm.LoadProject(prj_file);
            List<JObject> cfgs = new()
            {
                cm.CreateConfig("PFE_IM_LP")
            };

            cm.Picks(cfgs[^1], new List<JObject> {
                cm.MakePick("Rectifier", "Diode rectifier"),
                cm.MakePick("Motor", "Induction low power")
            });

            cfgs.Add(cm.CreateConfig("AFE_IM_LP"));
            cm.Picks(cfgs[^1], new List<JObject> {
                cm.MakePick("Rectifier", "Thyristor rectifier"),
                cm.MakePick("Motor", "Induction low power")
            });

            cfgs.Add(cm.CreateConfig("PFE_PMSM_LP"));
            cm.Picks(cfgs[^1], new List<JObject> {
                cm.MakePick("Rectifier", "Diode rectifier"),
                cm.MakePick("Motor", "PMSM low power")
            });

            cfgs.Add(cm.CreateConfig("AFE_PMSM_LP"));
            cm.Picks(cfgs[^1], new List<JObject> {
                cm.MakePick("Rectifier", "Thyristor rectifier"),
                cm.MakePick("Motor", "PMSM low power")
            });

            string[] cfg_file_list = Directory.GetFiles(cfgPath);

            foreach (string cfg_file in cfg_file_list)
            {
                cfgs.Add(cm.LoadConfig(cfg_file));
            }

            Console.WriteLine("Generating models:");
            for (int ind = 0; ind < cfgs.Count; ind++)
            {
                string cfg_name = cm.GetName(cfgs[ind]);
                Console.WriteLine((ind + 1) + " / " + cfgs.Count + " : " + cfg_name);
                cm.Generate(prj, cfgs[ind], outDir: outPath);
            }

            Console.WriteLine("Models are stored in the " + outPath + " folder.");
        }

        private static void Test29()
        {
            var cfg_manager = new ConfigurationManagerAPI();
            var t = cfg_manager.LoadProject("C:\\Users\\Dell\\source\\repos\\TyphoonHilApi\\TestData\\drive example\\example_project.cmp");
            Console.WriteLine(t);
        }

        private static void Test28()
        {
            var mdl = new SchematicAPI();
            mdl.CreateNewModel();
            var tr1 = mdl.CreateComponent("core/Three Phase Two Winding Transformer");
            mdl.SetComponentIconImage(tr1, "C:\\Users\\Dell\\Pictures\\Screenshots\\bumbar.png");
            mdl.SetColor(tr1, "red");
            mdl.RefreshIcon(tr1);
            mdl.SaveAs("C:\\Users\\Dell\\source\\repos\\TyphoonHilApi\\TestData\\bla.tse");
            mdl.CloseModel();
        }

        private static void Test27()
        {
            var mdl = new SchematicAPI();
            mdl.Load("C:\\ex.tse");
            Console.WriteLine(mdl.ModelToApi());
            mdl.CloseModel();
        }

        private static void Test25()
        {
            SchematicAPI mdl = new();
            mdl.CreateNewModel();

            JObject abs = mdl.CreateComponent("core/Abs", name: "Abs 1");

            Console.WriteLine(mdl.GetTerminalSpType(mdl.Term(abs, "out")));

            Console.WriteLine(mdl.GetTerminalSpType(mdl.Term(abs, "in")));

            JObject const1 = mdl.CreateComponent("core/Constant", name: "Constant 1");

            JObject probe1 = mdl.CreateComponent("core/Probe", name: "Probe 1");
            mdl.CreateConnection(mdl.Term(const1, "out"), mdl.Term(probe1, "in"));

            // After compile...
            Console.WriteLine(mdl.GetTerminalSpTypeValue(mdl.Term(probe1, "in")));

            mdl.CloseModel();
        }

        private static void Test24()
        {
            SchematicAPI mdl = new();
            mdl.CreateNewModel();

            JObject constItem = mdl.CreateComponent("core/Constant", name: "Constant 1");
            //Console.WriteLine(mdl.GetTerminalDimension(mdl.Term(constItem, "out")));

            mdl.SetTerminalDimension(mdl.Term(constItem, "out"), new Dimension(2));
            Console.WriteLine(mdl.GetTerminalDimension(mdl.Term(constItem, "out")));

            mdl.CloseModel();
        }

        private static void Test23()
        {
            var mdl = new SchematicAPI();
            mdl.CreateNewModel();

            var sub1 = mdl.CreateComponent("core/Subsystem", name: "Subsystem1");

            int newWidth = 70;
            int newHeight = 55;

            mdl.SetSize(sub1, width: newWidth, height: newHeight);
            Console.WriteLine(mdl.GetSize(sub1).Equals(new Size(newWidth, newHeight)));

            mdl.SetSize(sub1, width: 100);
            Console.WriteLine(mdl.GetSize(sub1).Equals(new Size(100, newHeight)));

            mdl.SetSize(sub1, height: 80);
            Console.WriteLine(mdl.GetSize(sub1).Equals(new Size(100, 80)));

            mdl.CloseModel();
        }

        private static void Test22()
        {
            var mdl = new SchematicAPI();
            mdl.CreateNewModel();

            var trLine = mdl.CreateComponent("core/Transmission Line", name: "TR Line 1");

            var modelDefHandle = mdl.Prop(trLine, "model_def");
            var modelDefComboValues = mdl.GetPropertyComboValues(modelDefHandle);
            var newComboValues = new List<string>(modelDefComboValues);
            newComboValues.Add("New option");

            mdl.SetPropertyComboValues(modelDefHandle, newComboValues);
            modelDefComboValues = mdl.GetPropertyComboValues(modelDefHandle);

            mdl.CloseModel();
        }

        private static void Test21()
        {
            SchematicAPI mdl = new();
            mdl.CreateNewModel();

            // Get position of tag item
            var tag = mdl.CreateTag(name: "Tag 1", value: "Tag value", position: new(160, 240));
            Console.WriteLine($"Tag position is {mdl.GetPosition(tag)}.");

            // Set position
            mdl.SetPosition(tag, new(800, 1600));
            Console.WriteLine($"New tag position is {mdl.GetPosition(tag)}.");

            mdl.CloseModel();
        }

        private static void Test20()
        {
            SchematicAPI mdl = new();
            mdl.CreateNewModel();

            // Create variable named 'var1'
            mdl.SetNamespaceVariable("var1", 20);

            Console.WriteLine(mdl.GetNamespaceVariable("var1"));

            // Update value for variable 'var1'
            mdl.SetNamespaceVariable("var1", 100);

            Console.WriteLine(mdl.GetNamespaceVariable("var1"));

            mdl.CloseModel();
        }

        private static void Test19()
        {
            SchematicAPI mdl = new();
            mdl.CreateNewModel();

            var r = mdl.CreateComponent("core/Resistor", name: "R1");
            var vm = mdl.CreateComponent("core/Voltage Measurement", name: "vm1");
            var tag = mdl.CreateTag(value: "A", name: "Tag 1");
            var sub1 = mdl.CreateComponent("core/Subsystem", name: "Subsystem 1");
            var innerL = mdl.CreateComponent("core/Inductor", parent: sub1, name: "Inner inductor");
            var innerPort = mdl.CreatePort(name: "Port 1", parent: sub1);
            var innerSub = mdl.CreateComponent("core/Subsystem", parent: sub1, name: "Inner subsystem");

            //
            // As GetItems was called without specifying a parent, top-level scheme items
            // will be returned.
            //
            var items = mdl.GetItems();
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }

            //
            // Demonstrating use of filtering with GetItems function.
            //
            // Get all ports from the subsystem referenced by sub1.
            items = mdl.GetItems(parent: sub1, itemType: ItemType.PORT);
            foreach (var item in items)
            {
                Console.WriteLine($"Item is {item}.");
                Console.WriteLine($"Item name part is {mdl.GetName(item)}.");
            }

            //
            // Get component terminals and properties.
            //
            var propHandles = mdl.GetItems(parent: r, itemType: ItemType.PROPERTY);
            Console.WriteLine($"Component '{mdl.GetName(r)}' property handles are '{string.Join(", ", propHandles)}'.");
            var termHandles = mdl.GetItems(parent: r, itemType: ItemType.TERMINAL);
            Console.WriteLine($"Component '{mdl.GetName(r)}' terminal handles are '{string.Join(", ", termHandles)}'.");

            mdl.CloseModel();
        }

        private static void Test18()
        {
            SchematicAPI mdl = new();
            mdl.CreateNewModel();

            var sub = mdl.CreateComponent("core/Subsystem", name: "Subsystem 1");
            var mask = mdl.CreateMask(sub);

            // Define icon drawing commands.
            string iconDrawingCommands = "image('my_image.png')";

            // Set mask icon drawing commands.
            mdl.SetIconDrawingCommands(mask, iconDrawingCommands);

            // Aquire current icon drawing commands.
            string maskIconDrawingCommands = mdl.GetIconDrawingCommands(mask);
            Console.WriteLine("Icon drawing commands are: " + maskIconDrawingCommands);

            mdl.CloseModel();
        }

        private static void Test17()
        {
            SchematicAPI mdl = new();
            mdl.CreateNewModel();

            var sub = mdl.CreateComponent("core/Subsystem", name: "Subsystem 1");

            //
            // Create mask and set MASK_INIT handler code.
            //
            var maskHandle = mdl.CreateMask(sub);

            string handlerCode = @"
                import time
                # Just display time.
                print(""Current time is '{0}'."".format(time.asctime()))
                ";
            mdl.SetHandlerCode(maskHandle, HandlerName.MASK_INIT, handlerCode);

            //
            // Create one property on mask and set its PROPERTY_VALUE_CHANGED handler.
            //
            var prop1 = mdl.CreateProperty(
                maskHandle,
                name: "prop_1",
                label: "Property 1",
                widget: Widget.COMBO,
                comboValues: new JArray() { "Choice 1", "Choice 2", "Choice 3" },
                tabName: "First tab"
            );

            // Set PROPERTY_VALUE_CHANGED handler on property.
            string proPvalueChangedHandlerCode = @"
                if (new_value == ""Choice 1"")
                {
                    Console.WriteLine(""It's a first choice."");
                }
                else if (new_value == ""Choice 2"")
                {
                    Console.WriteLine(""It's a second choice."");
                }
                else if (new_value == ""Choice 3"")
                {
                    Console.WriteLine(""It's a third choice"");
                }
                ";
            mdl.SetHandlerCode(prop1, HandlerName.PROPERTY_VALUE_CHANGED, proPvalueChangedHandlerCode);

            // Get handler code for a property prop1.
            string retrievedHandlerCode = mdl.GetHandlerCode(prop1, HandlerName.PROPERTY_VALUE_CHANGED);
            Console.WriteLine("Retrieved handler code is: " + retrievedHandlerCode);

            mdl.CloseModel();
        }

        private static void Test16()
        {
            SchematicAPI mdl = new();
            mdl.CreateNewModel();

            var constComponent = mdl.CreateComponent("core/Constant", name: "Constant 1");
            Console.WriteLine(mdl.GetConnectableDirection(mdl.Term(constComponent, "out")));

            var probeComponent = mdl.CreateComponent("core/Probe", name: "Probe 1");
            Console.WriteLine(mdl.GetConnectableDirection(mdl.Term(probeComponent, "in")));

            mdl.CloseModel();
        }

        private static void Test15()
        {
            SchematicAPI mdl = new();
            mdl.CreateNewModel();

            var constComponent = mdl.CreateComponent("core/Constant", name: "Constant 1");
            Console.WriteLine(mdl.GetConnectableKind(mdl.Term(constComponent, "out")));

            var resistorComponent = mdl.CreateComponent("core/Resistor", name: "Resistor 1");
            Console.WriteLine(mdl.GetConnectableKind(mdl.Term(resistorComponent, "p_node")));

            mdl.CloseModel();
        }

        private static void Test14()
        {
            SchematicAPI mdl = new();
            mdl.CreateNewModel();

            var const1 = mdl.CreateComponent("core/Constant", name: "Constant1");
            var const2 = mdl.CreateComponent("core/Constant", name: "Constant2");
            var junction = mdl.CreateJunction(kind: Kind.Sp);
            var sum1 = mdl.CreateComponent("core/Sum", name: "Sum1");
            var probe1 = mdl.CreateComponent("core/Probe", name: "Probe1");
            var probe2 = mdl.CreateComponent("core/Probe", name: "Probe2");

            var con1 = mdl.CreateConnection(mdl.Term(const1, "out"), junction);
            var con2 = mdl.CreateConnection(junction, mdl.Term(probe2, "in"));
            var con3 = mdl.CreateConnection(junction, mdl.Term(sum1, "in"));
            var con4 = mdl.CreateConnection(mdl.Term(const2, "out"), mdl.Term(sum1, "in1"));
            var con5 = mdl.CreateConnection(mdl.Term(sum1, "out"), mdl.Term(probe1, "in"));

            // Get items connected to component const1.
            foreach (var item in mdl.GetConnectedItems(const1))
            {
                Console.WriteLine(mdl.GetName(item));
            }

            // The same as above, but starting from junction.
            foreach (var item in mdl.GetConnectedItems(junction))
            {
                Console.WriteLine(mdl.GetName(item));
            }

            // Find all items connected to component sum1 "out" terminal.
            foreach (var item in mdl.GetConnectedItems(mdl.Term(sum1, "out")))
            {
                Console.WriteLine(mdl.GetName(item));
            }

            mdl.CloseModel();
        }

        private static void Test13()
        {
            SchematicAPI mdl = new();
            mdl.CreateNewModel();

            var constComponent = mdl.CreateComponent("core/Constant");
            var probeComponent = mdl.CreateComponent("core/Probe");
            var connection = mdl.CreateConnection(
                mdl.Term(constComponent, "out"),
                mdl.Term(probeComponent, "in"),
                breakpoints: new List<Position>
                {
                    new(100, 200),
                    new(100, 0)
                }
            );

            var breakpoints = mdl.GetBreakpoints(connection);
            Console.WriteLine("Breakpoints: " + string.Join(", ", breakpoints));

            mdl.CloseModel();
        }

        private static void Test12()
        {
            var mdl = new SchematicAPI();
            mdl.CreateNewModel();

            var parent_name = "Subsystem 1";
            var component_name = "Resistor 1";

            var comp_fqn = SchematicAPI.Fqn(parent_name, component_name);
            Console.WriteLine(comp_fqn);

            mdl.GetAvailableLibraryComponents().ForEach(Console.WriteLine);


            mdl.CloseModel();
        }

        private static void Test11()
        {
            var mdl = new SchematicAPI();
            mdl.CreateNewModel();

            var const1 = mdl.CreateComponent("core/Constant", name: "Constant 1");
            var junction = mdl.CreateJunction(kind: Kind.Sp, name: "Junction 1");
            var probe1 = mdl.CreateComponent("core/Probe", name: "Probe 1");
            var probe2 = mdl.CreateComponent("core/Probe", name: "Probe 2");
            var con1 = mdl.CreateConnection(mdl.Term(const1, "out"), junction);
            var con2 = mdl.CreateConnection(junction, mdl.Term(probe1, "in"));
            var con3 = mdl.CreateConnection(junction, mdl.Term(probe2, "in"));

            mdl.FindConnections(junction).ForEach(Console.WriteLine);
            Console.WriteLine("Another one");
            mdl.FindConnections(junction, mdl.Term(probe2, "in")).ForEach(Console.WriteLine);

            mdl.CloseModel();
        }

        private static void Test10()
        {
            SchematicAPI mdl = new();
            mdl.CreateNewModel();

            // Create component
            var constComponent = mdl.CreateComponent(typeName: "core/Constant");

            Console.WriteLine(mdl.IsPropertySerializable(mdl.Prop(constComponent, "value")));

            mdl.DisablePropertySerialization(mdl.Prop(constComponent, "value"));
            Console.WriteLine(mdl.IsPropertySerializable(mdl.Prop(constComponent, "value")));

            mdl.EnablePropertySerialization(mdl.Prop(constComponent, "value"));
            Console.WriteLine(mdl.IsPropertySerializable(mdl.Prop(constComponent, "value")));

            mdl.CloseModel();
        }

        private static void Test9()
        {
            SchematicAPI mdl = new();
            mdl.CreateNewModel();

            // Create component
            JObject r = mdl.CreateComponent("core/Resistor");

            // Disable property
            mdl.DisableProperty(mdl.Prop(r, "resistance"));

            // Check to see if property is enabled.
            Console.WriteLine(mdl.IsPropertyEnabled(mdl.Prop(r, "resistance")));

            // Enable property
            mdl.EnableProperty(mdl.Prop(r, "resistance"));

            Console.WriteLine(mdl.IsPropertyEnabled(mdl.Prop(r, "resistance")));

            mdl.CloseModel();
        }

        private static void Test8()
        {
            SchematicAPI mdl = new();

            string modelPath = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                "ex.tse"
            );

            modelPath = "C:\\Users\\Dell\\source\\repos\\TyphoonHilApi\\TestData\\RLC_example.tse";

            mdl.Load(modelPath);

            //
            // Names of items that can be disabled
            //
            List<string> itemDisableNames = new()
            {
                "L",
                "AI_1",
                "AI_2",
                "SM_1",
                "Probe1"
            };

            //
            // Names of subsystem [0] and item [1] inside subsystem
            //
            string subsystemName = "SS_1";
            string itemNameInsideSubsystem = "SM_6";

            //
            // Names of items that cannot be disabled
            //
            List<string> itemDontDisableNames = new()
            {
            "Subsystem1",
            "SM_5",
            "Min Max 1",
            "GA_2"
        };

            //
            // Fetch all items that can be disabled and that cannot be disabled
            //
            List<JObject?> itemsDisable = itemDisableNames.Select(itemName => mdl.GetItem(itemName)).ToList();
            List<JObject?> itemsDontDisable = itemDontDisableNames.Select(itemName => mdl.GetItem(itemName)).ToList();

            //
            // Disable, compile, enable - items that can be disabled
            //
            List<JObject> disabledItems = mdl.DisableItems(itemsDisable);
            mdl.Compile();
            List<JObject> affectedItems = mdl.EnableItems(disabledItems);
            mdl.Compile();

            //
            // Disable, compile, enable - items that cannot be disabled
            //
            disabledItems = mdl.DisableItems(itemsDontDisable);
            try
            {
                mdl.Compile();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            affectedItems = mdl.EnableItems(disabledItems!);
            mdl.Compile();

            //
            // Disable, compile, enable - items inside subsystem
            //
            JObject? parentItem = mdl.GetItem(subsystemName);
            JObject? concreteItem = mdl.GetItem(itemNameInsideSubsystem, parentItem);
            disabledItems = mdl.DisableItems(new() { concreteItem });
            try
            {
                mdl.Compile();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            affectedItems = mdl.EnableItems(new() { concreteItem });
            mdl.Compile();

            mdl.CloseModel();
        }

        private static void Test7()
        {
            SchematicAPI mdl = new();
            mdl.CreateNewModel();

            JObject? hwSett = mdl.DetectHwSettings();

            if (hwSett != null)
            {
                Console.WriteLine("HIL device was detected and model configuration was changed to {0}.", hwSett);
            }
            else
            {
                Console.WriteLine("HIL device autodetection failed, maybe HIL device is not connected.");
            }

            mdl.CloseModel();
        }

        private static void Test6()
        {
            SchematicAPI mdl = new();
            mdl.CreateNewModel();

            // Create some items and then delete them.
            JObject r = mdl.CreateComponent("core/Resistor");
            JObject j = mdl.CreateJunction();
            // var tag = mdl.CreateTag(value: "Val 1");
            JObject sub1 = mdl.CreateComponent("core/Subsystem");
            JObject innerPort = mdl.CreatePort(parent: sub1, name: "Inner port1");

            //
            // Delete items
            //
            mdl.DeleteItem(r);
            mdl.DeleteItem(j);
            //mdl.DeleteItem(tag);

            // Delete subsystem
            mdl.DeleteItem(sub1);

            mdl.CloseModel();
        }

        private static void Test5()
        {
            SchematicAPI mdl = new();
            mdl.CreateNewModel();

            JObject sub = mdl.CreateComponent("core/Subsystem", name: "Subsystem 1");
            JObject mask = mdl.CreateMask(sub);

            Console.WriteLine("Created mask is: '{0}'.", mask);

            mdl.CloseModel();
        }

        private static void Test4()
        {
            SchematicAPI mdl = new();

            string filePath = Path.Combine(
                Path.GetDirectoryName(Path.GetFullPath(Environment.GetCommandLineArgs()[0]))!,
                "create_library_model_lib",
                "example_library.tlib"
            );

            string libName = "Example Library";

            mdl.CreateLibraryModel(
                libName,
                filePath
            );

            //
            // Create basic components, connect them and add them to the library
            //
            JObject r = mdl.CreateComponent("core/Resistor", name: "R1");
            JObject c = mdl.CreateComponent("core/Capacitor", name: "C1");

            JObject con = mdl.CreateConnection(mdl.Term(c, "n_node"),
                                              mdl.Term(r, "p_node"));

            JObject con1 = mdl.CreateConnection(mdl.Term(c, "p_node"),
                                               mdl.Term(r, "n_node"));

            //
            // Save the library, load it and try saving the loaded library
            //
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            }
            mdl.SaveAs(filePath);
            mdl.Load(filePath);
            mdl.Save();
            Console.WriteLine(filePath);
        }

        private static void Test3()
        {
            SchematicAPI mdl = new();
            mdl.CreateNewModel();

            // Create comment with some text
            JObject comment1 = mdl.CreateComment("This is a comment");

            //
            // Create comment with text, custom name, and specified position
            // in the scheme.
            //
            JObject comment2 = mdl.CreateComment("This is a comment 2", name: "Comment 2", position: new(100, 200));

            Console.WriteLine("Comment is {0}.", comment2);

            mdl.CloseModel();
        }

        private static void Test2()
        {
            SchematicAPI mdl = new();
            mdl.CreateNewModel();

            //
            // Library file is located in directory 'custom_lib' one level above this
            // example file.
            //

            string directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!;
            string libPath = System.IO.Path.Combine(directory, "custom_lib");

            // Get all current library paths and remove them
            List<string> oldPaths = mdl.GetLibraryPaths();
            oldPaths.ForEach(x => mdl.RemoveLibraryPath(x));

            // Add library path and reload library to be able to use the added library.
            mdl.AddLibraryPath(libPath);
            mdl.ReloadLibraries();

            // Create components from loaded libraries.
            JObject comp = mdl.CreateComponent("my_lib/CustomComponent");
            Console.WriteLine($"Component is '{comp}'.");

            JObject comp2 = mdl.CreateComponent("archived_user_lib/CustomComponent1");
            Console.WriteLine($"Second component (from archived library) is '{comp2}'.");

            // Remove library from the path.
            mdl.RemoveLibraryPath(libPath);

            // Add again the previous library paths
            foreach (string path in oldPaths)
            {
                mdl.AddLibraryPath(path);
            }

            mdl.CloseModel();
        }

        private static void Test1()
        {
            string path = "C:\\Users\\Dell\\source\\repos\\TyphoonHilApi\\TyphoonHilApiTests\\ProtectedData\\";

            // Create SchematicAPI object
            SchematicAPI model = new();

            // Create new model
            model.CreateNewModel("Scratch");

            // Starting coordinates
            int x0 = 8192;
            int y0 = 8192;

            // Component values
            double rInValue = 100.0;
            double lValue = 1e-5;
            double rValue = 0.1;
            double cValue = 5e-4;

            Console.WriteLine("Creating scheme items...");

            // Create Voltage Source component
            JObject vIn = model.CreateComponent("core/Voltage Source", name: "Vin", position: new(x0 - 300, y0), rotation: "right");

            // Create Resistor component
            JObject rIn = model.CreateComponent("core/Resistor", name: "Rin", position: new(x0 - 200, y0 - 100));

            // Create Current Measurement component
            JObject iMeas = model.CreateComponent("core/Current Measurement", name: "I", position: new(x0 - 100, y0 - 100));

            // Create Ground component
            JObject gnd = model.CreateComponent("core/Ground", name: "gnd", position: new(x0 - 300, y0 + 200));

            // Create Inductor component
            JObject ind = model.CreateComponent("core/Inductor", name: "L", position: new(x0, y0), rotation: Rotation.Right);

            // Create Voltage Measurement component
            JObject vMeas = model.CreateComponent("core/Voltage Measurement", name: "V", position: new(x0 + 200, y0), rotation: Rotation.Right);

            // Create RC Load Subsystem component
            JObject rcLoad = model.CreateComponent("core/Empty Subsystem", name: "RC Load", position: new(x0 + 100, y0));

            // Create port in Subsystem
            JObject p1 = model.CreatePort(name: "P1", parent: rcLoad, terminalPosition: new TerminalPosition(TerminalPosition.Top, TerminalPosition.Auto), rotation: Rotation.Right, position: new(x0, y0 - 200));

            // Create port in Subsystem
            JObject p2 = model.CreatePort(name: "P2", parent: rcLoad, terminalPosition: new TerminalPosition(TerminalPosition.Bottom, TerminalPosition.Auto), rotation: Rotation.Left, position: new(x0, y0 + 200));

            // Create Resistor component
            JObject r = model.CreateComponent("core/Resistor", parent: rcLoad, name: "R", position: new(x0, y0 - 50), rotation: Rotation.Right);

            // Create Capacitor component
            JObject c = model.CreateComponent("core/Capacitor", parent: rcLoad, name: "C", position: new(x0, y0 + 50), rotation: Rotation.Right);

            // Create necessary junctions
            JObject junction1 = model.CreateJunction(name: "J1", position: new(x0 - 300, y0 + 100));

            JObject junction2 = model.CreateJunction(name: "J2", position: new(x0, y0 - 100));

            JObject junction3 = model.CreateJunction(name: "J3", position: new(x0, y0 + 100));

            JObject junction4 = model.CreateJunction(name: "J4", position: new(x0 + 100, y0 - 100));

            JObject junction5 = model.CreateJunction(name: "J5", position: new(x0 + 100, y0 + 100));

            // Connect all the components
            Console.WriteLine("Connecting components...");
            model.CreateConnection(model.Term(vIn, "p_node"), model.Term(rIn, "p_node"));
            model.CreateConnection(model.Term(vIn, "n_node"), junction1);
            model.CreateConnection(model.Term(gnd, "node"), junction1);
            model.CreateConnection(model.Term(rIn, "n_node"), model.Term(iMeas, "p_node"));
            model.CreateConnection(model.Term(iMeas, "n_node"), junction2);
            model.CreateConnection(junction2, model.Term(ind, "p_node"));
            model.CreateConnection(model.Term(ind, "n_node"), junction3);
            model.CreateConnection(junction1, junction3);
            model.CreateConnection(junction2, junction4);
            model.CreateConnection(junction3, junction5);
            model.CreateConnection(model.Term(rcLoad, "P1"), junction4);
            model.CreateConnection(junction5, model.Term(rcLoad, "P2"));
            model.CreateConnection(junction4, model.Term(vMeas, "p_node"));
            model.CreateConnection(model.Term(vMeas, "n_node"), junction5);
            model.CreateConnection(p1, model.Term(r, "p_node"));
            model.CreateConnection(model.Term(r, "n_node"), model.Term(c, "p_node"));
            model.CreateConnection(model.Term(c, "n_node"), p2);

            // Set component parameters
            Console.WriteLine("Setting component properties...");
            model.SetPropertyValue(model.Prop(rIn, "resistance"), rInValue);
            model.SetPropertyValue(model.Prop(ind, "inductance"), lValue);
            model.SetPropertyValue(model.Prop(r, "resistance"), rValue);
            model.SetPropertyValue(model.Prop(c, "capacitance"), cValue);


            // Save the model
            string fileName = path + "RLC_example.tse";
            Console.WriteLine($"Saving model to '{fileName}'...");
            model.SaveAs(fileName);

            // Compile model
            if (model.Compile())
                Console.WriteLine("Model successfully compiled.");
            else
                Console.WriteLine("Model failed to compile");

            // Close the model
            model.CloseModel();
        }
    }
}
