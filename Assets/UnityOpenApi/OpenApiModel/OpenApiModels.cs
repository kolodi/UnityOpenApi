using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

namespace UnityOpenApi.ApiModels.OpenApi
{
    [Serializable]
    public class OpenApi3
    {
        public string openapi;
        public OpenApi3Info info;
        public List<OpenApi3Server> servers = new List<OpenApi3Server>();
        public Dictionary<string, Dictionary<OpenApi3HttpWords, OpenApi3Method>> paths = new Dictionary<string, Dictionary<OpenApi3HttpWords, OpenApi3Method>>();
        public OpenApi3Components components;
    }

    [Serializable]
    public struct OpenApi3Info
    {
        public string title;
        public string version;
    }

    [Serializable]
    public struct OpenApi3Server
    {
        public string url;
        public Dictionary<string, OpenApi3ServerVariable> variables;
    }

    [Serializable]
    public struct OpenApi3ServerVariable
    {
        public string @default;
        public List<string> @enum;
    }

    [Serializable]
    public class OpenApi3Paths
    {
        [JsonProperty("$ref")]
        public string @ref;
        public string summary;
        public string description;
        public OpenApi3Operation get;
        public OpenApi3Operation put;
        public OpenApi3Operation post;
        public OpenApi3Operation delete;
        public OpenApi3Operation options;
        public OpenApi3Operation head;
        public OpenApi3Operation patch;
        public OpenApi3Operation trace;
        public List<OpenApi3Server> servers;
        public List<OpenApi3MethodParameter> parameters;
    }

    [Serializable]
    public struct OpenApi3Operation
    {
        public List<OpenApi3MethodParameter> parameters;
        public Dictionary<string, OpenApi3MethodResponse> responses;
        public List<Dictionary<string, string[]>> security;
    }

    [Serializable]
    public enum OpenApi3HttpWords
    {
        get, post, put, patch, delete, head, options, trace
    }

    [Serializable]
    public enum OpenApi3ParameterIn
    {
        query, path, header, cookie
    }
        
    [Serializable]
    public struct OpenApi3Method
    {
        public List<OpenApi3MethodParameter> parameters;
        public Dictionary<string, OpenApi3MethodResponse> responses;
        public List<Dictionary<string, string[]>> security;
    }

    [Serializable]
    public class OpenApi3MethodParameter
    {
        public string name;
        [JsonProperty("in")]
        public OpenApi3ParameterIn @in;
        public bool required;
    }

    [Serializable]
    public struct OpenApi3MethodResponse
    {

    }

    [Serializable]
    public struct OpenApi3Components
    {
        public Dictionary<string, OpenApi3Schema> schemas;
        public Dictionary<string, OpenApi3SecurityScheme> securitySchemes;
    }

    [Serializable]
    public struct OpenApi3SecurityScheme
    {
        public string type;
        public string name;
        public OpenApi3ParameterIn @in;
    }

    [Serializable]
    public struct OpenApi3Schema
    {
        public OpenApi3SchemaType type;
    }

    [Serializable]
    public enum OpenApi3SchemaType
    {
        @string, number, integer, @object, array, boolean, @null
    }

}