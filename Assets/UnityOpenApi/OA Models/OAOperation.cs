using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public int OperationCurrentHash
        {
            get
            {
                // NOTE: the following does not guarantee the uniqueness of the hash :(
                int h = OperationType.GetHashCode();
                h += RequestBody.LastRequestBody.GetHashCode();
                ParametersValues.ForEach(pm =>
                {
                    if (pm.HasValue) h += pm.value.GetHashCode();
                });

                return h;
            }
        }

        public OAOperation(OperationType operationType, OpenApiOperation op, PathItemAsset pathItemAsset)
        {
            pathAsset = pathItemAsset;
            OperationId = op.OperationId;
            OperationType = (AOOperationType)operationType;
            Summary = op.Summary;
            Description = op.Description;
            Deprecated = op.Deprecated;

            if (op.Parameters.Count > 0)
            {
                Parameters = op.Parameters.Select(p => new OAParameter(p)).ToList();
            }
            else
            {
                Parameters = pathItemAsset.Parameters;
            }

            ParametersValues = Parameters.Select(p => new ParameterValue { parameter = p }).ToList();

            Servers = op.Servers.Select(s => new OAServer(s)).ToList();

            Tags = op.Tags.Select(t => new OATag(t)).ToList();

            RequestBody = new OARequestBody(op.RequestBody);

            ExternalDocs = new OAExternalDocs(op.ExternalDocs);
        }

        public void SetParameterValue(string parameterName, string val)
        {
            var parVal = ParametersValues.FirstOrDefault(p => p.parameter.Name == parameterName);
            if(parVal == null)
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