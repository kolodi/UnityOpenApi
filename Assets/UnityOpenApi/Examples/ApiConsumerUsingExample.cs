using UnityEngine;
using UnityOpenApi;
using PetStore;
using System.Collections.Generic;
using Newtonsoft.Json;
using Proyecto26;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ApiConsumerUsingExample : MonoBehaviour
{
    public void OnResult(ResponseHelper r)
    {
        List<Pet> pets = JsonConvert.DeserializeObject<List<Pet>>(r.Text);
        pets.ForEach(pet =>
        {
            Debug.Log("Pet ID: " + pet.id + ", type: " + pet.type + ", price: " + pet.price);
        });
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(ApiConsumerUsingExample))]
public class ApiConsumerUsingExampleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var t = (ApiConsumerUsingExample)target;
        if(GUILayout.Button("Test"))
        {
            var api = t.GetComponent<ApiConsumer>();
            Operation op = api.Operation;

            api.UpdateFromApi();
        }
    }
}
#endif
