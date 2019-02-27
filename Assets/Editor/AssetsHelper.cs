using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetsHelper
{
    public static T GetOrCreateScriptableObject<T>(string path, string subpath, string ext = ".asset", bool setDirty = true) where T : ScriptableObject
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
        } else
        {
            Debug.Log("Getting reference to the existing asset at: " + assetFullPath);
        }
        if (setDirty)
        {
            EditorUtility.SetDirty(asset);
        }
        return asset;
    }
}
