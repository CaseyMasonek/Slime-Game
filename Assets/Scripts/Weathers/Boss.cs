using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Boss : WeatherEvent
{
    public override string EventName { get; } = "Bossfight";
    public override string Description { get; set; } = "Good Luck.";

    [SerializeField] private GameObject pref;
    [SerializeField] private Vector3 pos = new Vector3(40,35);
    
    void Start()
    {
        Instantiate(Resources.Load("Boss"), pos, Quaternion.identity);
        var g = Instantiate(pref, pos, Quaternion.identity);
        Debug.Log(g.transform.position);
    }
}
