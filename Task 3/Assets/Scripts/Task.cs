using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public string task;

    public Task(string task)
    {
        this.task = task;
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}