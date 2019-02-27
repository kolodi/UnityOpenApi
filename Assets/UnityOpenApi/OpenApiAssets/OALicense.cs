using Microsoft.OpenApi.Models;
using System;

namespace UnityOpenApi
{
    [Serializable]
    public class OALicense
    {
        public string Name;
        public string Url;
        public bool Present = true;

        public OALicense(OpenApiLicense openApiLicense)
        {
            if(openApiLicense==null)
            {
                Present = false;
                return;
            }
            Name = openApiLicense.Name;
            Url = openApiLicense.Url.ToString();
        }
    }
}