using System;
using System.Collections.Generic;

namespace UnityOpenApi
{
    [Serializable]
    public class OAServerVariable
    {
        public string Name;
        public string Description;
        public string Default;
        public List<string> Enum;
        public int Current;
    }
}