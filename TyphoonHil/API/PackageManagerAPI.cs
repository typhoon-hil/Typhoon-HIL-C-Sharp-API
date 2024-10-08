﻿using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using TyphoonHil.Exceptions;

namespace TyphoonHil.API
{
    public class PackageDto
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public bool IsPackage { get; set; }

        internal PackageDto(JObject packageAsJObject)
        {
            Name = (string)packageAsJObject["package_name"];
            Version = (string)packageAsJObject["version"];
            IsPackage = (bool)packageAsJObject["package"];
        }

        public PackageDto()
        {
            Name = Version = "";
        }

        public override string ToString()
        {
            return $"Package({Name}, {Version})";
        }
    }

    public class PackageManagerAPI : AbstractAPI
    {
        protected override int ProperPort => Ports.PackageManagerApiPort;

        protected override JObject HandleRequest(string method, JObject parameters)
        {
            var res = Request(method, parameters);
            if (!res.ContainsKey("error")) return res;
            var msg = res["error"] == null ? null : (string)res["error"]["message"];
            throw new PackageManagerAPIException(msg);
        }

        public string CreateExample(string title, string modelFile, string panelFile, string outputPath,
            List<string> tags = null, string description = "", string imageFile = "", string appNoteFile = "",
            List<string> tests = null, List<string> testResources = null, List<string> resources = null)
        {
            var parameters = new JObject
            {
                { "title", title },
                { "model_file", modelFile },
                { "panel_file", panelFile },
                { "output_path", outputPath },
                { "tags", tags == null ? null : new JArray(tags) },
                { "description", description },
                { "image_file", imageFile },
                { "app_note_file", appNoteFile },
                { "tests", tests == null ? null : new JArray(tests) },
                { "test_resources", testResources == null ? null : new JArray(testResources) },
                { "resources", resources == null ? null : new JArray(resources) }
            };

            return (string)HandleRequest("create_example", parameters)["result"];
        }

        public string CreatePackage(string packageName, string version, string outputPath, string author = "",
            string description = "", string minimalSwVersion = "", List<string> libraryPaths = null,
            List<string> resourcePaths = null, List<string> examplePaths = null,
            List<string> additionalFilesPaths = null, List<string> pythonPackagesPaths = null,
            List<string> documentationPaths = null, string documentationLandingPage = "", string releaseNotesPath = "")
        {
            var parameters = new JObject
            {
                { "package_name", packageName },
                { "version", version },
                { "output_path", outputPath },
                { "author", author },
                { "description", description },
                { "minimal_sw_version", minimalSwVersion },
                { "library_paths", libraryPaths == null ? null : new JArray(libraryPaths) },
                { "resource_paths", resourcePaths == null ? null : new JArray(resourcePaths) },
                { "example_paths", examplePaths == null ? null : new JArray(examplePaths) },
                { "additional_files_paths", additionalFilesPaths == null ? null : new JArray(additionalFilesPaths) },
                { "python_packages_paths", pythonPackagesPaths == null ? null : new JArray(pythonPackagesPaths) },
                { "documentation_paths", documentationPaths == null ? null : new JArray(documentationPaths) },
                { "documentation_landing_page", documentationLandingPage },
                { "release_notes_path", releaseNotesPath }
            };

            return (string)HandleRequest("create_package", parameters)["result"];
        }

        private static List<PackageDto> GeneratePackages(JArray packageContainer)
        {
            return packageContainer.Select(package => new PackageDto((JObject)package)).ToList();
        }

        public List<PackageDto> GetInstalledPackages()
        {
            var parameters = new JObject();

            return GeneratePackages((JArray)HandleRequest("get_installed_packages", parameters)["result"]);
        }

        public List<PackageDto> GetModifiedPackages()
        {
            var parameters = new JObject();

            return GeneratePackages((JArray)HandleRequest("get_modified_packages", parameters)["result"]);
        }

        public void InstallPackage(string filename)
        {
            var parameters = new JObject
            {
                { "filename", filename }
            };

            HandleRequest("install_package", parameters);
        }

        public void ReinstallPackage(string packageName)
        {
            var parameters = new JObject
            {
                { "package_name", packageName }
            };

            HandleRequest("reinstall_package", parameters);
        }

        public void UninstallPackage(string packageName)
        {
            var parameters = new JObject
            {
                { "package_name", packageName }
            };

            HandleRequest("uninstall_package", parameters);
        }

        public void ValidatePackage(string filename)
        {
            var parameters = new JObject
            {
                { "filename", filename }
            };

            HandleRequest("validate_package", parameters);
        }
    }
}