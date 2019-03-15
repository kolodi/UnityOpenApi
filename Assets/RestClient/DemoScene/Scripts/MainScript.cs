using UnityEngine;
using UnityEditor;
using Models;
using Proyecto26;
using System.Collections.Generic;

public class MainScript : MonoBehaviour {

    private readonly string basePath = "https://jsonplaceholder.typicode.com";
	private RequestHelper currentRequest;

	public void Get(){

		// We can add default request headers for all requests
		RestClient.DefaultRequestHeaders["Authorization"] = "Bearer ...";

        RequestHelper requestOptions = null;

		RestClient.GetArray<Post>(basePath + "/posts").Then(res => {
            EditorUtility.DisplayDialog ("Posts", JsonHelper.ArrayToJsonString<Post>(res, true), "Ok");
            return RestClient.GetArray<Todo>(basePath + "/todos");
		}).Then(res => {
            EditorUtility.DisplayDialog ("Todos", JsonHelper.ArrayToJsonString<Todo>(res, true), "Ok");
            return RestClient.GetArray<User>(basePath + "/users");
		}).Then(res => {
			EditorUtility.DisplayDialog ("Users", JsonHelper.ArrayToJsonString<User>(res, true), "Ok");


			// We can add specific options and override default headers for a request
			requestOptions = new RequestHelper { 
				Uri = basePath + "/photos",
				Headers = new Dictionary<string, string> {
					{ "Authorization", "Other token..." }
				},
				EnableDebug = true
			};
			return RestClient.GetArray<Photo>(requestOptions);
		}).Then(res => {
			EditorUtility.DisplayDialog("Header", requestOptions.GetHeader("Authorization"), "Ok");

			// And later we can clean the default headers for all requests
			RestClient.CleanDefaultHeaders();

		}).Catch(err => EditorUtility.DisplayDialog ("Error", err.Message, "Ok"));
	}

	public void Post(){

		currentRequest = new RequestHelper {
			Uri = basePath + "/posts",
			Body = new Post {
				title = "foo",
				body = "bar",
				userId = 1
			}
		};
		RestClient.Post<Post>(currentRequest)
		.Then(res => EditorUtility.DisplayDialog ("Success", JsonUtility.ToJson(res, true), "Ok"))
		.Catch(err => EditorUtility.DisplayDialog ("Error", err.Message, "Ok"));
	}

	public void Put(){

		currentRequest = new RequestHelper {
			Uri = basePath + "/posts/1", 
			Body = new Post {
				title = "foo",
				body = "bar",
				userId = 1
			},
			Retries = 5,
			RetrySecondsDelay = 1,
			RetryCallback = (err, retries) => {
				Debug.Log (string.Format ("Retry #{0} Status {1}\nError: {2}", retries, err.StatusCode, err));
			}
		};
		RestClient.Put<Post>(currentRequest, (err, res, body) => {
			if(err != null){
				EditorUtility.DisplayDialog ("Error", err.Message, "Ok");
			}
			else{
				EditorUtility.DisplayDialog ("Success", JsonUtility.ToJson(body, true), "Ok");
			}
		});
	}

	public void Delete(){

		RestClient.Delete(basePath + "/posts/1", (err, res) => {
			if(err != null){
				EditorUtility.DisplayDialog ("Error", err.Message, "Ok");
			}
			else{
				EditorUtility.DisplayDialog ("Success", "Status: " + res.StatusCode.ToString(), "Ok");
			}
		});
	}

	public void AbortRequest(){
		if (currentRequest != null) {
			currentRequest.Abort();
			currentRequest = null;
		}
	}
}