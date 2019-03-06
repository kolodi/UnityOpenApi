using System;

namespace UnityOpenApi
{
    [Serializable]
    public enum OAParameterLocation
    {
        Query = 0,
        Header = 1,
        Path = 2,
        Cookie = 3
    }
}