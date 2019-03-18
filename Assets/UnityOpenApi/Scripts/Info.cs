using System;

namespace UnityOpenApi
{
    [Serializable]
    public class Info
    {
        public string Title;
        public string Description;
        public string Version;
        public string TermsOfService;
        public Contact Contact;
        public License License;
    }
}