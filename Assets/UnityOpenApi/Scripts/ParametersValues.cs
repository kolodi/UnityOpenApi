using System;

namespace UnityOpenApi
{
    [Serializable]
    public class ParameterValue
    {
        public OAParameter parameter;
        public string value;
        public bool HasValue
        {
            get
            {
                return string.IsNullOrEmpty(value) == false;
            }
        }
    }
}