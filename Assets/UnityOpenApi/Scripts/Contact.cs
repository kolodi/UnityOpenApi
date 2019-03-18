using System;
using UnityEditor;

namespace UnityOpenApi
{
    [Serializable]
    public class Contact
    {
        public string Name;
        public string Url;
        public string Email;
        public bool Present;
    }
}