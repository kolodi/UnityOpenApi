using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HttpMono;

public class TestMono : MonoBehaviour
{

    /// <summary>
    /// If you want it to work not during the play in Editor,
    /// you can use editor coroutines package: 
    /// https://docs.unity3d.com/Packages/com.unity.editorcoroutines@0.0/manual/index.html
    /// </summary>
    [ContextMenu("Test")]
    private void Test()
    {
        StopCoroutine(Test1());
        StartCoroutine(Test1());
    }

    private IEnumerator Test1()
    {
        HttpRequestHandler httpHandler = new HttpRequestHandler();
        yield return httpHandler.Get("https://jsonplaceholder.typicode.com/users");
        Debug.Log(httpHandler.Result.Text);
    }

    
}
