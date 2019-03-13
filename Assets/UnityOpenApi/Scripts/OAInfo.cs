using System;

namespace UnityOpenApi
{
    [Serializable]
    public class OAInfo
    {
        public string Title;
        public string Description;
        public string Version;
        public string TermsOfService;
        public OAContact Contact;
        public OALicense License;
    }
}