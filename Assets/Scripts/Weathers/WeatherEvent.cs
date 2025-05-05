using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeatherEvent : MonoBehaviour
{
    public abstract string EventName { get; }
    public abstract string Description { get; set; }
}
