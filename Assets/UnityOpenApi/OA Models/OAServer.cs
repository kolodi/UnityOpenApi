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

            Variables.ForEach(v =>
            {
                v.Default = v.Default.Trim('/');
                if (v.Enum.Count == 0)
                {
                    v.Enum = new List<string> { v.Default };
                } else
                {
                    // trim all slashes for variants
                    v.Enum = new List<string>(v.Enum.Select(e => e.Trim('/')));
                }
                v.Current = 0;
            });
        }
    }
}