using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Boss : WeatherEvent
{
    public override string EventName { get; } = "The Weatherman";
    public override string Description { get; } = "Brace yourself";

    [SerializeField] private GameObject pref;
    [SerializeField] private Vector3 pos = new Vector3(-40,-21);
    
    void Start()
    {
        Instantiate(Resources.Load("Boss"), pos, Quaternion.identity);
        var g = Instantiate(pref, pos, Quaternion.identity);
        Debug.Log(g.transform.position);
    }
}
