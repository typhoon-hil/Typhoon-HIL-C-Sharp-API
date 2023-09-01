﻿using Newtonsoft.Json.Linq;
using TyphoonHilApi.Communication.Exceptions;

namespace TyphoonHilApi.Communication
{
    internal abstract class AbsractAPI
    {
        public abstract int ProperPort { get; }
        private ICommunication _communication { get; set; }
        public PortsDto Ports { get; set; }

        public AbsractAPI(ICommunication communication)
        {
            this._communication = communication;
            Ports = _communication.Discover();
        }

        public AbsractAPI()
        {
            this._communication = new NetMQCommunication();
            Ports = _communication.Discover();
        }

        public JObject Request(string method, JObject parameters)
        {
            return _communication.Request(method, parameters, ProperPort);
        }

        protected abstract JObject HandleRequest(string method, JObject parameters);

        protected JObject HandleRequest(string method)
        {
            return HandleRequest(method, new());
        }

    }
}
