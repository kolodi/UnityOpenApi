using System;

namespace UnityOpenApi
{
    [Serializable]
    public class OARequestBody
    {
        public string Description;
        public bool Required;

        public string LastRequestBody;
    }
}