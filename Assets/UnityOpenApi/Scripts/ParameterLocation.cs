using System;

namespace UnityOpenApi
{
    [Serializable]
    public enum ParameterLocation
    {
        Query = 0,
        Header = 1,
        Path = 2,
        Cookie = 3
    }
}