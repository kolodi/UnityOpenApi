using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOpenApi;
using PetStore;

public class PetshopConsumeExample : MonoBehaviour
{
    [Header("Get Pets")]
    [SerializeField] PathItemAsset pets = null;
    [SerializeField] string tags = "cat";
    [SerializeField] int limit = 3;

    [Header("Get Single Pet")]
    [SerializeField] PathItemAsset pet = null;
    [SerializeField] string petIdToGet = "5";

    [ContextMenu("Get Pets")]
    public void GetPets()
    {
        var operation = pets.GetOperation(AOOperationType.Get);
        operation.SetParameterValue("tags", tags);
        operation.SetParameterValue("limit", limit.ToString());

        pets.ExecuteOperation<List<Pet>>(operation, pets =>
        {
            pets.ForEach(pet =>
            {
                Debug.Log("Pet ID: " + pet.Id + ", type: " + pet.Type + ", price: " + pet.Price);
            });
        });
    }

    [ContextMenu("Get Single Pet")]
    public void GetPet()
    {
        var operation = pet.GetOperation(AOOperationType.Get);
        operation.SetParameterValue("id", petIdToGet);

        pet.ExecuteOperation<Pet>(operation, pet =>
        {
            Debug.Log("Pet ID: " + pet.Id + ", type: " + pet.Type + ", price: " + pet.Price);
        });
    }
}
