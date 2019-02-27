using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PetStore
{

    public class Pet
    {
        public int Id { get; set; }
        public PetType Type { get; set; }
        public float Price { get; set; }
    }

    public enum PetType
    {
        dog, cat, fish, bird, gecko
    }
}