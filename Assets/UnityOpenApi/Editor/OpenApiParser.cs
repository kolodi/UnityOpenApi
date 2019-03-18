﻿using UnityEngine;
using UnityEditor;
using Microsoft.OpenApi.Readers;
using System.IO;
using Microsoft.OpenApi.Validations;
using Microsoft.OpenApi.Models;
using UnityOpenApi;
using System.Linq;
using System.Collections.Generic;
using Proyecto26;
using RSG;

[CreateAssetMenu(menuName = "Unity Open API/Parser")]
public class OpenApiParser : ScriptableObject
{
    [SerializeField]
    [HideInInspector]
    private TextAsset textAsset = null;

    private string _lastAssetPath = "Assets";

    public void ParseFromAsset()
    {
        var doc = Parse(textAsset.text);
        GenerateAssets(doc);
    }

    public OpenApiDocument Parse(string json)
    {
        var stream = CreateStream(json);
        var parsed = new OpenApiStreamReader(new OpenApiReaderSettings
        {
            ReferenceResolution = ReferenceResolutionSetting.ResolveLocalReferences,
            RuleSet = ValidationRuleSet.GetDefaultRuleSet()
        }).Read(stream, out var openApiDiagnostic);
        Debug.Log("Successfully parsed API Description: " + parsed.Info.Title);
        return parsed;
    }

    public void ParseFromUrl(string url)
    {
        RestClient.Get(url)
        .Then(result =>
        {
            var doc = Parse(result.Text);
            GenerateAssets(doc);
        })
       .Catch(err => Debug.LogError(err.Message));
    }

    private MemoryStream CreateStream(string text)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);

        writer.Write(text);
        writer.Flush();
        stream.Position = 0;

        return stream;
    }

    public void GenerateAssets(OpenApiDocument doc)
    {
        string assetsPath = EditorUtility.OpenFolderPanel("Select assets folder", _lastAssetPath, "");
        _lastAssetPath = assetsPath;
        assetsPath = assetsPath.Substring(assetsPath.IndexOf("Assets"));
        ApiAsset apiAsset = AssetsHelper.GetOrCreateScriptableObject<ApiAsset>(assetsPath, doc.Info.Title);

        #region ApiAsset update
        apiAsset.info = new Info()
        {
            Title = doc.Info.Title,
            Description = doc.Info.Description,
            Version = doc.Info.Version,
            TermsOfService = doc.Info.TermsOfService == null ? "" : doc.Info.TermsOfService.ToString(),
            Contact = CreateContact(doc.Info.Contact),
            License = CreateLicence(doc.Info.License),
        };

        apiAsset.externalDocs = CreateExternalDocs(doc.ExternalDocs);

        apiAsset.servers = doc.Servers.Select(s => CreateAOServer(s)).ToList();


        apiAsset.pathItemAssets = new List<PathItemAsset>();

        #endregion

        foreach (var p in doc.Paths)
        {
            string fileName = p.Key.Replace('/', '_');
            PathItemAsset a = AssetsHelper.GetOrCreateScriptableObject<PathItemAsset>(assetsPath, fileName);
            a.ApiAsset = apiAsset;

            #region path item update


            a.Path = p.Key;

            a.Summary = p.Value.Summary;
            a.Description = p.Value.Description;
            a.Parameters = p.Value.Parameters.Select(par => CreateAOParameter(par)).ToList();
            a.Operations = p.Value.Operations.Select(o => CreateAOOperation(o.Key, o.Value, a)).ToList();
            a.Servers = p.Value.Servers.Select(s => CreateAOServer(s)).ToList();

            #endregion

            apiAsset.pathItemAssets.Add(a);
        }



        AssetDatabase.SaveAssets();
    }

    private Parameter CreateAOParameter(OpenApiParameter openApiParameter)
    {
        return new Parameter()
        {
            Name = openApiParameter.Name,
            Required = openApiParameter.Required,
            AllowReserved = openApiParameter.AllowReserved,
            Explode = openApiParameter.Explode,
            AllowEmptyValue = openApiParameter.AllowEmptyValue,
            Deprecated = openApiParameter.Deprecated,
            Description = openApiParameter.Description,
            UnresolvedReference = openApiParameter.UnresolvedReference,

            In = (UnityOpenApi.ParameterLocation)openApiParameter.In,

            Reference = CreateReference(openApiParameter.Reference),
        };
    }

    private Operation CreateAOOperation(OperationType operationType, OpenApiOperation op, PathItemAsset pathItemAsset)
    {
        var operation = new Operation()
        {
            pathAsset = pathItemAsset,
            OperationId = op.OperationId,
            OperationType = (HttpWord)operationType,
            Summary = op.Summary,
            Description = op.Description,
            Deprecated = op.Deprecated,

            Parameters = op.Parameters.Count > 0 ?
            op.Parameters.Select(p => CreateAOParameter(p)).ToList() :
            pathItemAsset.Parameters,


            Servers = op.Servers.Select(s => CreateAOServer(s)).ToList(),

            Tags = op.Tags.Select(t => CreateOATag(t)).ToList(),

            RequestBody = CreateOARequestBody(op.RequestBody),

            ExternalDocs = CreateExternalDocs(op.ExternalDocs),
        };
        operation.ParametersValues = operation.Parameters.Select(p => new ParameterValue { parameter = p }).ToList();

        return operation;
    }

    private Tag CreateOATag(OpenApiTag openApiTag)
    {
        return new Tag()
        {
            Name = openApiTag.Name,
            Description = openApiTag.Description,
            ExternalDocs = CreateExternalDocs(openApiTag.ExternalDocs),
        };
    }

    public RequestBody CreateOARequestBody(OpenApiRequestBody requestBody)
    {
        if (requestBody == null) return new RequestBody();

        return new RequestBody()
        {
            Description = requestBody.Description,
            Required = requestBody.Required,
        };
    }

    private Reference CreateReference(OpenApiReference openApiReference)
    {
        if (openApiReference == null) return new Reference();

        return new Reference()
        {
            IsPresent = true,
            ExternalResource = openApiReference.ExternalResource,
            Type = (UnityOpenApi.ReferenceType)openApiReference.Type,
            Id = openApiReference.Id,
            IsExternal = openApiReference.IsExternal,
            IsLocal = openApiReference.IsLocal,
            reference = string.IsNullOrEmpty(openApiReference.ReferenceV2)
            ? openApiReference.ReferenceV3 : openApiReference.ReferenceV2,
        };
    }

    private Contact CreateContact(OpenApiContact openApiContact)
    {
        if (openApiContact == null)
            return new Contact();

        return new Contact()
        {
            Url = openApiContact.Url.ToString(),
            Name = openApiContact.Name,
            Email = openApiContact.Email,
            Present = true,
        };
    }

    private License CreateLicence(OpenApiLicense openApiLicense)
    {
        if (openApiLicense == null) return new License();

        return new License()
        {
            Name = openApiLicense.Name,
            Url = openApiLicense.Url.ToString(),
            Present = true,
        };
    }

    private ExternalDocs CreateExternalDocs(OpenApiExternalDocs ExternalDocs)
    {
        if (ExternalDocs != null)
        {
            return new ExternalDocs()
            {
                Description = ExternalDocs.Description,
                Url = ExternalDocs.Url.ToString(),
            };
        }
        return new ExternalDocs();
    }

    private Server CreateAOServer(OpenApiServer s)
    {
        var server = new Server()
        {
            Description = s.Description,
            Url = s.Url,
            Variables = s.Variables.ToDictionary(v => v.Key, v => new ServerVariable()
            {
                Name = v.Key,
                Description = v.Value.Description,
                Default = v.Value.Default,
                Enum = new List<string>(v.Value.Enum),
                Current = v.Value.Enum.IndexOf(v.Value.Default)
            }).Values.ToList(),
        };
        server.Variables.ForEach(v =>
        {
            v.Default = v.Default.Trim('/');
            if (v.Enum.Count == 0)
            {
                v.Enum = new List<string> { v.Default };
            }
            else
            {
                // trim all slashes for variants
                v.Enum = new List<string>(v.Enum.Select(e => e.Trim('/')));
            }
            v.Current = 0;
        });
        return server;
    }

}

[CustomEditor(typeof(OpenApiParser))]
public class OpenApiParserEditor : Editor
{
    string url = "https://raw.githubusercontent.com/OAI/OpenAPI-Specification/master/examples/v3.0/uspto.yaml";
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);

        var t = (OpenApiParser)target;

        GUILayout.BeginVertical();
        var a = serializedObject.FindProperty("textAsset");
        EditorGUILayout.ObjectField(a);
        serializedObject.ApplyModifiedProperties();
        if (GUILayout.Button("Parse From Asset"))
        {
            t.ParseFromAsset();
        }
        GUILayout.EndVertical();

        GUILayout.Space(10);

        GUILayout.BeginVertical();
        url = EditorGUILayout.TextField(new GUIContent("URL"), url);
        if (GUILayout.Button("Parse From Url"))
        {
            t.ParseFromUrl(url);
        }
        GUILayout.EndVertical();

        EditorUtility.SetDirty(target);
    }
}
