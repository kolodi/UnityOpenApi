using System;

namespace UnityOpenApi
{
    [Serializable]
    public enum ReferenceType
    {
        Schema = 0,
        Response = 1,
        Parameter = 2,
        Example = 3,
        RequestBody = 4,
        Header = 5,
        SecurityScheme = 6,
        Link = 7,
        Callback = 8,
        Tag = 9
    }
}