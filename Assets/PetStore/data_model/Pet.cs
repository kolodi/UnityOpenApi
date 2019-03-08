using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PetStore
{
    [Serializable]
    public class Pet
    {
        public int Id;
        public PetType Type;
        public float Price;
    }

    [Serializable]
    public class NewPet
    {
        public string Name;
        public PetType Type;
        public float Price;
    }

    [Serializable]
    public enum PetType
    {
        dog, cat, fish, bird, gecko
    }
}