using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityOpenApi;
using UnityOpenApi.ApiModels.OpenApi;


public class ApiAssetsGenerator
{
    public void GenerateOrUpdateApiAssets(string basePath, OpenApi3 openApi3, bool considerOptionHttpWord = false)
    {

        string apiConfigAssetFilename = openApi3.info.title;
        string localApiDirectory = CreateDirectoryIfNotExists(basePath, openApi3.info.title);

        // create paths subdirectory
        string resDirectoryPath = CreateDirectoryIfNotExists(localApiDirectory, "resources");

        // create data model directory
        string dataModelsDirectory = CreateDirectoryIfNotExists(localApiDirectory, "data_model");


        // Create/update api config
        ApiConfig apiConfig = GetOrCreateAsset<ApiConfig>(localApiDirectory, apiConfigAssetFilename);

        apiConfig.rawData = openApi3;

        //apiConfig.baseUri = openApi3.servers[0].url.Replace(
        //    "/{basePath}"
        //    , openApi3.servers[0].variables.basePath._default);

        // for now consider only first server
        apiConfig.serverVariableValues = new List<ApiConfig.ServerVariableValue>();
        foreach (var sv in openApi3.servers[0].variables)
        {
            apiConfig.serverVariableValues.Add(new ApiConfig.ServerVariableValue()
            {
                varName = sv.Key,
                varValue = sv.Value.@default
            });
        }

        // Authorization resources
        apiConfig.authorizers = new List<ApiAuthorizer>();
        if (openApi3.components.securitySchemes != null)
        {
            foreach (var s in openApi3.components.securitySchemes)
            {
                switch (s.Value.@in)
                {
                    case OpenApi3ParameterIn.query:
                        break;
                    case OpenApi3ParameterIn.path:
                        break;
                    case OpenApi3ParameterIn.header:
                        string fName = apiConfigAssetFilename + "_auth_" + s.Key;
                        ApiHeaderAuthorizer hAuth = GetOrCreateAsset<ApiHeaderAuthorizer>(localApiDirectory, fName);
                        hAuth.authName = s.Key;
                        hAuth.securityScheme = s.Value;
                        apiConfig.authorizers.Add(hAuth);
                        break;
                    case OpenApi3ParameterIn.cookie:
                        break;
                    default:
                        break;
                }
            }
        }

        // create/update individual api resources
        foreach (var openApiPath in openApi3.paths)
        {

            string resSubdirectory = CreateDirectoryIfNotExists(resDirectoryPath, openApiPath.Key);

            //string resUri = apiConfig.baseUri;
            //if (openApiPath.Key != "/") resUri += openApiPath.Key;

            foreach (var res in openApiPath.Value)
            {
                string fName = apiConfig.name
                    + openApiPath.Key.Replace("/", "_")
                    + "_"
                    + res.Key.ToString().ToUpper();

                if (considerOptionHttpWord == false && res.Key == OpenApi3HttpWords.options)
                {
                    Debug.Log("Skipping OPTION http resource for " + fName);
                    continue;
                }

                ApiResource apiResource = GetOrCreateAsset<ApiResource>(resSubdirectory, fName);


                apiResource.apiConfig = apiConfig;
                apiResource.httpWord = res.Key;
                apiResource.path = openApiPath.Key;
                apiResource.method = res.Value;
                apiResource.ResetParametersValues();

                if (res.Value.security != null && res.Value.security.Count > 0)
                {
                    string authName = res.Value.security[0].First().Key;
                    apiResource.apiAuthorizer = apiConfig.GetAuthorizer(authName);
                    if (apiResource.apiAuthorizer == null)
                    {
                        Debug.LogError("Can't find authorizer " + authName + " for " + apiResource.name);
                    }
                }

                EditorUtility.SetDirty(apiResource);
            }
        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

    }

    private T GetOrCreateAsset<T>(string path, string subpath, string ext = ".asset", bool setDirty = true) where T : ScriptableObject
    {
        if (path[path.Length - 1] != '/') path += "/";
        if (subpath[0] == '/') subpath = subpath.Substring(1);
        string assetFullPath = path + subpath + ext;
        T asset = AssetDatabase.LoadAssetAtPath<T>(assetFullPath);
        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<T>();
            Debug.Log("Creating new asset at: " + assetFullPath);
            AssetDatabase.CreateAsset(asset, assetFullPath);
        }
        if (setDirty)
        {
            EditorUtility.SetDirty(asset);
        }
        return asset;
    }

    private string CreateDirectoryIfNotExists(params string[] paths)
    {
        string path = string.Empty;
        foreach (var p in paths)
        {
            if (string.IsNullOrEmpty(p))
            {
                continue;
            }
            string pathPart = p;
            char separator = '/';
            if (pathPart[0] != separator)
            {
                pathPart = separator + pathPart;
            }
            if (pathPart[pathPart.Length - 1] == separator)
            {
                pathPart = pathPart.Remove(pathPart.Length - 1);
            }
            path += pathPart;
        }
        // remove the initial slash
        path = path.Substring(1);
        if (Directory.Exists(path) == false)
        {
            Debug.Log("Creating directory: " + path);
            Directory.CreateDirectory(path);
        }
        return path;
    }
}
