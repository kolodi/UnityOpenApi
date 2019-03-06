using Microsoft.OpenApi.Models;
using System;
using UnityEditor;

namespace UnityOpenApi
{
    [Serializable]
    public class OAContact
    {
        public string Name;
        public string Url;
        public string Email;
        public bool Present = true;

        public OAContact (OpenApiContact openApiContact)
        {
            if(openApiContact == null)
            {
                Present = false;
                return;
            }
            Url = openApiContact.Url.ToString();
            Name = openApiContact.Name;
            Email = openApiContact.Email;
        }
    }
}