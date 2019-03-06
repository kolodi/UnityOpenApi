using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
        //public OpenApiRequestBody RequestBody;      
        public List<OAParameter> Parameters;
        public List<ParameterValue> ParametersValues;
        //public OpenApiExternalDocs ExternalDocs;      
        public string Description;
        public string Summary;
        public List<OATag> Tags;
        public List<OAServer> Servers;
        public PathItemAsset pathAsset;

        public OAOperation(OperationType operationType, OpenApiOperation openApiOperation, PathItemAsset pathItemAsset)
        {
            pathAsset = pathItemAsset;
            OperationId = openApiOperation.OperationId;
            OperationType = (AOOperationType)operationType;
            Summary = openApiOperation.Summary;
            Description = openApiOperation.Description;
            Deprecated = openApiOperation.Deprecated;

            if (openApiOperation.Parameters.Count > 0)
            {
                Parameters = openApiOperation.Parameters.Select(p => new OAParameter(p)).ToList();
            }
            else
            {
                Parameters = pathItemAsset.Parameters;
            }

            ParametersValues = Parameters.Select(p => new ParameterValue { parameter = p }).ToList();

            Servers = openApiOperation.Servers.Select(s => new OAServer(s)).ToList();

            Tags = openApiOperation.Tags.Select(t => new OATag(t)).ToList();
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
    }
}