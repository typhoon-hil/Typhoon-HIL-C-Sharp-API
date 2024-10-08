﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TyphoonHil.API;
using TyphoonHilTests.Utils;

namespace TyphoonHilTests.API
{
    [TestClass]
    public class ConfigurationManagerAPITests
    {
        public ConfigurationManagerAPITests() { }

        public ConfigurationManagerAPI Model { get; set; }
        public string StartupPath { get; set; }
        public string TestDataPath { get; set; }
        public string ProtectedDataPath { get; set; }

        [TestInitialize]
        public void Init()
        {
            Model = new ConfigurationManagerAPI();
            StartupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            TestDataPath = Path.Combine(StartupPath, "TestData");
            ProtectedDataPath = Path.Combine(StartupPath, "ProtectedData");

            if (Directory.Exists(TestDataPath)) TestUtils.ClearDirectory(TestDataPath);
        }

        [TestMethod]
        public void GeneralTest()
        {
            var basePath = Path.Combine(ProtectedDataPath, "drive example");
            var prjFile = Path.Combine(basePath,"example_project.cmp");
            var cfgPath = Path.Combine(basePath,"configs");
            var outPath = Path.Combine(basePath,"output");

            var prj = Model.LoadProject(prjFile);
            List<JObject> cfgs = new List<JObject>()
            {
                Model.CreateConfig("PFE_IM_LP")
            };

            Model.Picks(cfgs[cfgs.Count - 1], new List<JObject>
            {
                Model.MakePick("Rectifier", "Diode rectifier"),
                Model.MakePick("Motor", "Induction low power")
            });

            cfgs.Add(Model.CreateConfig("AFE_IM_LP"));
            Model.Picks(cfgs[cfgs.Count - 1], new List<JObject>
            {
                Model.MakePick("Rectifier", "Thyristor rectifier"),
                Model.MakePick("Motor", "Induction low power")
            });

            cfgs.Add(Model.CreateConfig("PFE_PMSM_LP"));
            Model.Picks(cfgs[cfgs.Count - 1], new List<JObject>
            {
                Model.MakePick("Rectifier", "Diode rectifier"),
                Model.MakePick("Motor", "PMSM low power")
            });

            cfgs.Add(Model.CreateConfig("AFE_PMSM_LP"));
            Model.Picks(cfgs[cfgs.Count - 1], new List<JObject>
            {
                Model.MakePick("Rectifier", "Thyristor rectifier"),
                Model.MakePick("Motor", "PMSM low power")
            });

            var cfgFileList = Directory.GetFiles(cfgPath);

            cfgs.AddRange(cfgFileList.Select(cfgFile => Model.LoadConfig(cfgFile)));

            Console.WriteLine("Generating models:");
            for (var ind = 0; ind < cfgs.Count; ind++)
            {
                var cfgName = Model.GetName(cfgs[ind]);
                Console.WriteLine(ind + 1 + " / " + cfgs.Count + " : " + cfgName);
                //Model.Generate(prj, cfgs[ind], outPath);
            }

            Console.WriteLine("Models are stored in the " + outPath + " folder.");
        }
    }
}