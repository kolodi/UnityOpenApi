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
        public bool Present;
    }
}