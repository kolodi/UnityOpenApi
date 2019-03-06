using Microsoft.OpenApi.Models;
using System;

namespace UnityOpenApi
{
    [Serializable]
    internal class OAInfo
    {
        public string Title;
        public string Description;
        public string Version;
        public string TermsOfService;
        public OAContact Contact;
        public OALicense License;

        public OAInfo (OpenApiInfo openApiInfo)
        {
            Title = openApiInfo.Title;
            Description = openApiInfo.Description;
            Version = openApiInfo.Version;
            TermsOfService = openApiInfo.TermsOfService == null ? "" : openApiInfo.TermsOfService.ToString();
            Contact = new OAContact(openApiInfo.Contact);
            License = new OALicense(openApiInfo.License);
        }
    }
}