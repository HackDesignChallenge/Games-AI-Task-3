﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PythonBridge : MonoBehaviour
{
    void Start()
    {
        //StartCoroutine(GetText("https://z1.data-qubit.com/todos/todo1"));

        Task t = new Task("Unity Task");
        StartCoroutine(PostText(JsonUtility.ToJson(t)));

    }

    IEnumerator PostText(string jsonString)
    {
        UnityWebRequest request = UnityWebRequest.Put("https://z1.data-qubit.com/todos/todo2", jsonString);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.Send();
    }

    IEnumerator GetText(string uri)
    {
        UnityWebRequest www = UnityWebRequest.Get(uri);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }
}