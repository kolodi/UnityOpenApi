using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityOpenApi
{
    [Serializable]
    public class OAServer
    {
        public string Description;
        public string Url;
        public List<OAServerVariable> Variables;
    }
}