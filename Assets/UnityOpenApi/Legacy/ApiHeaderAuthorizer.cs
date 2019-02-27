using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityOpenApi
{
    /// <summary>
    /// Use this as a container to save authentication token
    /// </summary>
    [CreateAssetMenu(menuName = "Unity Open API/API Header Authorizer")]
    public class ApiHeaderAuthorizer : ApiAuthorizer
    {
        [SerializeField]
        private string token = string.Empty;
        public string Token { get => token; set => token = value; }
    }
}
