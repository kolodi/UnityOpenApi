using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityOpenApi
{
    [Serializable]
    public class Server
    {
        public string Description;
        public string Url;
        public List<ServerVariable> Variables;
    }
}