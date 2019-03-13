using Microsoft.OpenApi.Models;
using System;

namespace UnityOpenApi
{
    [Serializable]
    public class OALicense
    {
        public string Name;
        public string Url;
        public bool Present;
    }
}