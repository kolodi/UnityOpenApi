using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsontTestObject
{

    public enum TestEnum
    {
        V1,
        V2,
        V333
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public TestEnum SomeKind { get; private set; }

    [JsonProperty("player_name")]
    public string Name { get; set; }

    [JsonProperty("position")]
    private Vector3 Position { get; set; }

    [JsonProperty("rotation")]
    [JsonConverter(typeof(QuaternionConverter))]
    private Quaternion Rotation { get; set; }

    [JsonProperty("creation_date")]
    public DateTime CreationDate { get; }

    [JsonProperty("expiration_date")]
    public DateTime ExpirationDate { get; }

    public JsontTestObject ()
    {
        Position = new Vector3(1, 2, 3);
        Rotation = Quaternion.Euler(10.0f, 20.0f, 50.0f);
        CreationDate = DateTime.Now;
        ExpirationDate = CreationDate + TimeSpan.FromDays(10);
    }
}
