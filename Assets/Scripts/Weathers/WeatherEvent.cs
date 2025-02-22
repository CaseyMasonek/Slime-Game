using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeatherEvent : MonoBehaviour
{
    public abstract string eventName { get; protected set; }
    public abstract string description { get; protected set; }
}
