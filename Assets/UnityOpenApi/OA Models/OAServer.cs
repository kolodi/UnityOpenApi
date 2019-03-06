using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityOpenApi
{
    [Serializable]
    public class OAServer
    {
        public string Description;
        public string Url;
        public List<OAServerVariable> Variables;

        public OAServer(OpenApiServer s)
        {
            Description = s.Description;
            Url = s.Url;
            Variables = s.Variables.ToDictionary(v => v.Key, v => new OAServerVariable()
            {
                Name = v.Key,
                Description = v.Value.Description,
                Default = v.Value.Default,
                Enum = new List<string>(v.Value.Enum),
                Current = v.Value.Enum.IndexOf(v.Value.Default)
            }).Values.ToList();
        }
    }
}