using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json.Linq;

namespace TyphoonHil.Communication
{
    internal class NetMQCommunication : ICommunication, IDisposable
    {
        private readonly object _syncRoot = new object();

        // Cache of persistent RequestSockets per port
        private readonly Dictionary<int, RequestSocket> _requestSockets = new Dictionary<int, RequestSocket>();

        private bool _disposed;

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
                        {
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
                                    SchematicApiPort = apiPorts["sch_api"] == null
                                        ? 0
                                        : apiPorts["sch_api"]["server_rep_port"].Value<int>(),
                                    HilApiPort = apiPorts["hil_api"] == null
                                        ? 0
                                        : apiPorts["hil_api"]["server_rep_port"].Value<int>(),
                                    ScadaApiPort = apiPorts["scada_api"] == null
                                        ? 0
                                        : apiPorts["scada_api"]["server_rep_port"].Value<int>(),
                                    PvGenApiPort = apiPorts["pv_gen_api"] == null
                                        ? 0
                                        : apiPorts["pv_gen_api"]["server_rep_port"].Value<int>(),
                                    FwApiPort = apiPorts["fw_api"] == null
                                        ? 0
                                        : apiPorts["fw_api"]["server_rep_port"].Value<int>(),
                                    ConfigurationManagerApiPort = apiPorts["configuration_manager_api"] == null
                                        ? 0
                                        : apiPorts["configuration_manager_api"]["server_rep_port"].Value<int>(),
                                    DeviceManagerApiPort = apiPorts["device_manager_api"] == null
                                        ? 0
                                        : apiPorts["device_manager_api"]["server_rep_port"].Value<int>(),
                                    PackageManagerApiPort = apiPorts["package_manager_api"] == null
                                        ? 0
                                        : apiPorts["package_manager_api"]["server_rep_port"].Value<int>()
                                };
                                return ports;
                            }
                            else
                            {
                                requestRetries--;
                            }
                        }

                        if (j == 0)
                            RunThcc();
                    }
                }
            }

            throw new Exception("Typhoon HIL service registry not found in port range.");
        }

        /// <summary>
        /// Main request method – now reuses persistent RequestSockets per port.
        /// </summary>
        public JObject Request(string method, JObject parameters, int port)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(NetMQCommunication));

            var message = CreateMessage(method, parameters);

            lock (_syncRoot)
            {
                var socket = GetOrCreateRequestSocket(port);

                // Send & receive over the persistent socket
                socket.SendFrame(message.ToString());

                // Can be switched to TryReceiveFrameString with timeout if desired
                var answer = socket.ReceiveFrameString();

                if (answer == null)
                    throw new Exception("Received null response from THCC over NetMQ.");

                return JObject.Parse(answer);
            }
        }

        /// <summary>
        /// Creates or returns an existing RequestSocket for the given port.
        /// </summary>
        private RequestSocket GetOrCreateRequestSocket(int port)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(NetMQCommunication));

            if (_requestSockets.TryGetValue(port, out var existing) && !existing.IsDisposed)
            {
                return existing;
            }

            // Create a new persistent socket for this port
            var reqSocket = new RequestSocket();
            reqSocket.Connect($"tcp://localhost:{port}");

            // Tweak options
            reqSocket.Options.Linger = TimeSpan.FromMilliseconds(0);

            _requestSockets[port] = reqSocket;
            return reqSocket;
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

            // Split the environment variable using the path separator
            var paths = varValue.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            // Ensure there is at least one path in the environment variable
            if (paths.Length == 0)
                throw new Exception("No valid paths found in TYPHOONPATH");

            // Take the first path (newest installed version)
            var newestVersionPath = paths[0];

            // Create the path to the executable
            var exePath = Path.Combine(newestVersionPath, "typhoon_hil.exe");

            // Check if the executable exists
            if (!File.Exists(exePath))
                throw new Exception($"Executable not found at path: {exePath}");

            // Start the THCC
            var startInfo = new ProcessStartInfo(exePath)
            {
                WorkingDirectory = newestVersionPath
            };

            Process.Start(startInfo);
        }

        /// <summary>
        /// Dispose persistent sockets when the API is disposed or app shuts down.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;

            lock (_syncRoot)
            {
                foreach (var socket in _requestSockets.Values)
                {
                    try
                    {
                        socket?.Dispose();
                    }
                    catch
                    {
                        // ignore cleanup errors
                    }
                }

                _requestSockets.Clear();
                _disposed = true;
            }

            // Cleanup NetMQ global resources
            NetMQConfig.Cleanup();
        }
    }
}
