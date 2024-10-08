﻿using System;
using System.Diagnostics;
using System.IO;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json.Linq;

namespace TyphoonHil.Communication
{
    internal class NetMQCommunication : ICommunication
    {
        public PortsDto Discover(int startPort = 50000, int endPort = 50100, int requestRetries = 30, int timeout = 1000)
        {
            var requestRetriesInit = requestRetries;
            for (var j = 0; j < 2; j++)
            {
                requestRetries = requestRetriesInit;
                var offset = endPort - startPort + 1;
                using (var socket = new SubscriberSocket())
                {
                    for (var i = 0; i < offset; i++)
                    {
                        var port = startPort + i;
                        socket.Connect($"tcp://localhost:{port}");
                    }

                    socket.Subscribe("");
                    socket.Options.Linger = TimeSpan.FromMilliseconds(0);

                    socket.Poll(PollEvents.PollIn, TimeSpan.FromMilliseconds(timeout));
                    using (var poller = new NetMQPoller())
                    {
                        poller.Add(socket);

                        while (requestRetries != 0)
                            if (socket.Poll(PollEvents.PollIn, TimeSpan.FromMilliseconds(timeout)) == PollEvents.PollIn)
                            {
                                var res = socket.ReceiveFrameString();
                                if (res == null) break;
                                var parsedRes = JObject.Parse(res);
                                var result = parsedRes["result"].Value<JArray>();
                                var header = result[0].ToString();
                                if (header != "typhoon-service-registry") continue;

                                var apiPorts = result[2].ToObject<JObject>();

                                var ports = new PortsDto
                                {
                                    SchematicApiPort = apiPorts["sch_api"] == null ? 
                                        0 : apiPorts["sch_api"]["server_rep_port"].Value<int>(),
                                    HilApiPort = apiPorts["hil_api"] == null ? 
                                        0 : apiPorts["hil_api"]["server_rep_port"].Value<int>(),
                                    ScadaApiPort = apiPorts["scada_api"] == null ? 
                                        0 : apiPorts["scada_api"]["server_rep_port"].Value<int>(),
                                    PvGenApiPort = apiPorts["pv_gen_api"] == null ? 
                                        0 : apiPorts["pv_gen_api"]["server_rep_port"].Value<int>(),
                                    FwApiPort = apiPorts["fw_api"] == null ? 
                                        0 : apiPorts["fw_api"]["server_rep_port"].Value<int>(),
                                    ConfigurationManagerApiPort = apiPorts["configuration_manager_api"] == null ? 
                                            0 : apiPorts["configuration_manager_api"]["server_rep_port"].Value<int>(),
                                    DeviceManagerApiPort = apiPorts["device_manager_api"] == null ? 
                                        0 : apiPorts["device_manager_api"]["server_rep_port"].Value<int>(),
                                    PackageManagerApiPort = apiPorts["package_manager_api"] == null ? 
                                        0 : apiPorts["package_manager_api"]["server_rep_port"].Value<int>()
                                };
                                return ports;
                            }
                            else
                            {
                                requestRetries--;
                            }

                        if (j == 0)
                            RunThcc();
                    }
                }
            }

            throw new Exception();
        }

        public JObject Request(string method, JObject parameters, int port)
        {
            var message = CreateMessage(method, parameters);

            using (var reqSocket = new RequestSocket())
            {
                // Connect to the server
                reqSocket.Connect($"tcp://localhost:{port}");
                reqSocket.SendFrame(message.ToString());

                var answer = reqSocket.ReceiveFrameString();
                reqSocket.Close();
                return JObject.Parse(answer);
            }
        }

        public static JObject GenerateMessageBase()
        {
            JObject message = new JObject()
            {
                { "api", "1.0" },
                { "jsonrpc", "2.0" },
                { "id", Guid.NewGuid().ToString() }
            };

            return message;
        }

        private static JObject CreateMessage(string method, JToken parameters)
        {
            var message = GenerateMessageBase();
            message.Add("method", method);
            message.Add("params", parameters);
            return message;
        }

        private static void RunThcc()
        {
            const string varName = "TYPHOONPATH";
            var varValue = Environment.GetEnvironmentVariable(varName) ?? throw new Exception("THCC does not exist");
            var typhoonHilRoot = varValue.Remove(varValue.Length - 1);
            var exePath = Path.Combine(typhoonHilRoot, "typhoon_hil.exe");

            var startInfo = new ProcessStartInfo(exePath)
            {
                WorkingDirectory = typhoonHilRoot
            };

            Process.Start(startInfo);
        }
    }
}