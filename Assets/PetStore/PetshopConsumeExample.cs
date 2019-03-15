using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOpenApi;
using PetStore;
using Newtonsoft.Json;

public class PetshopConsumeExample : MonoBehaviour
{
    [Header("Get Pets")]
    [SerializeField] PathItemAsset pets = null;
    [SerializeField] string tags = "cat";
    [SerializeField] int limit = 3;

    [Header("Get Single Pet")]
    [SerializeField] PathItemAsset pet = null;
    [SerializeField] string petIdToGet = "5";

    [Header("New pet")]
    [SerializeField] NewPet newPet = null;

    [ContextMenu("Get Pets")]
    public void GetPets()
    {
        var operation = pets.GetOperation(AOOperationType.GET);
        operation.SetParameterValue("tags", tags);
        operation.SetParameterValue("limit", limit.ToString());
        operation.ignoreCache = true;

        pets.ExecuteOperation<List<Pet>>(operation)
            .Then(pets =>
            {
                pets.ForEach(pet =>
                {
                    Debug.Log("Pet ID: " + pet.id + ", type: " + pet.type + ", price: " + pet.price);
                });
            })
            .Catch(err =>
            {
                Debug.Log(err);
            });
    }

    [ContextMenu("Get Single Pet")]
    public void GetPet()
    {
        var operation = pet.GetOperation(AOOperationType.GET);
        operation.SetParameterValue("id", petIdToGet);

        pet.ExecuteOperation<Pet>(operation)
            .Then(pet =>
            {
                Debug.Log("Pet ID: " + pet.id + ", type: " + pet.type + ", price: " + pet.price);
            });
    }

    [ContextMenu("Create Pet")]
    public void CreatePet()
    {
        var operation = pets.GetOperation(AOOperationType.POST);
        var serialized = JsonConvert.SerializeObject(newPet);
        operation.SetRequestBody(serialized);

        pets.ExecuteOperation<NewPetResponse>(operation)
            .Then(r =>
            {
                var pet = r.pet;
                Debug.Log("Pet name: " + pet.name + ", type: " + pet.type + ", price: " + pet.price);
            });
    }

    [ContextMenu("Test Hash")]
    void TestHash()
    {
        var operation = pets.GetOperation(AOOperationType.POST);
        Debug.Log(operation.OperationCurrentHash);
    }
}
