using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PetStore
{
    [Serializable]
    public class Pet
    {
        public int id;
        public PetType type;
        public float price;
    }

    [Serializable]
    public class NewPet
    {
        public string name;
        [JsonConverter(typeof(StringEnumConverter))]
        public PetType type;
        public float price;
    }

    [Serializable]
    public class NewPetResponse
    {
        public NewPet pet;
        public string message;
    }

    [Serializable]
    public enum PetType
    {
        dog, cat, fish, bird, gecko
    }
}