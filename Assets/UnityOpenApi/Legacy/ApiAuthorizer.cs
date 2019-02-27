using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOpenApi.ApiModels.OpenApi;

namespace UnityOpenApi
{
    public abstract class ApiAuthorizer : ScriptableObject
    {
        public string authName;
        public OpenApi3SecurityScheme securityScheme;
    }
}
