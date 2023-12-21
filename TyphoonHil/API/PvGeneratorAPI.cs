using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using TyphoonHil.Exceptions;

namespace TyphoonHil.API
{
    public class PvModelType
    {
        public const string Detailed = "Detailed";
        public const string En50530 = "EN50530 Compatible";
        public const string NormalizedIv = "Normalized IV";
    }

    public class PvResponse
    {
        public PvResponse(JArray jArray)
        {
            Status = (bool)jArray[0];
            Message = jArray[1] == null ? null : (string)jArray[1];
        }

        public bool Status { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return $"{Status}, {Message}";
        }
    }

    public class PvGeneratorAPI : AbstractAPI
    {
        public List<string> DetailedPvType = new List<string>() { "cSi", "Amorphous Si" };
        public List<string> En50530PvTypes = new List<string>() { "cSi", "Thin film", "User defined" };

        protected override int ProperPort => Ports.PvGenApiPort;

        protected override JObject HandleRequest(string method, JObject parameters)
        {
            var res = Request(method, parameters);
            if (!res.ContainsKey("error")) return res;
            var msg = res["error"] == null ? null : (string)res["error"]["message"];
            throw new PvGeneratorAPIException(msg);
        }

        public PvResponse GeneratePvSettingsFile(string modelType, string fileName, JObject parameters)
        {
            var requestParameters = new JObject
        {
            { "modelType", modelType },
            { "fileName", fileName },
            { "parameters", parameters }
        };
            PvResponse res = new PvResponse((JArray)HandleRequest("generate_pv_settings_file", requestParameters)["result"]);
            Console.WriteLine(res);
            return res;
        }
    }
}