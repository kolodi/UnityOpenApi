using System;

namespace UnityOpenApi
{
    [Serializable]
    public class RequestBody
    {
        public string Description;
        public bool Required;

        public string LastRequestBody;
    }
}