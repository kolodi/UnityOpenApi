using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityOpenApi
{
    [Serializable]
    public class OAOperation
    {

        public string OperationId;
        public AOOperationType OperationType;
        //public IList<OpenApiSecurityRequirement> Security;      
        public bool Deprecated;
        //public IDictionary<string, OpenApiCallback> Callbacks;
        //public OpenApiResponses Responses;      
        public OARequestBody RequestBody;
        public List<OAParameter> Parameters;
        public List<ParameterValue> ParametersValues;
        public OAExternalDocs ExternalDocs;
        public string Description;
        public string Summary;
        public List<OATag> Tags;
        public List<OAServer> Servers;
        public PathItemAsset pathAsset;
        [SerializeField] private string cache;
        public string Cache
        {
            get { return cache; }
            set
            {
                cache = value;
            }
        }
        public bool ignoreCache = false;

        public bool GetFromCache(out string cache)
        {
            if (string.IsNullOrEmpty(this.Cache))
            {
                cache = string.Empty;
                return false;
            }

            cache = this.Cache;
            return true;
        }

        public int OperationCurrentHash
        {
            get
            {
                // NOTE: the following does not guarantee the uniqueness of the hash :(
                int h = OperationType.GetHashCode();

                if (string.IsNullOrEmpty(RequestBody.LastRequestBody) == false)
                    h += RequestBody.LastRequestBody.GetHashCode();

                ParametersValues.ForEach(pm =>
                {
                    if (pm.HasValue) h += pm.value.GetHashCode();
                });

                return h;
            }
        }


        public void SetParameterValue(string parameterName, string val)
        {
            var parVal = ParametersValues.FirstOrDefault(p => p.parameter.Name == parameterName);
            if (parVal == null)
            {
                throw new Exception("No parameter with name <" + parameterName + "> found in operation " + OperationId);
            }
            parVal.value = val;
        }

        public void SetRequestBody(string body)
        {
            RequestBody.LastRequestBody = body;
        }
    }
}