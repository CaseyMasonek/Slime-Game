using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class None : WeatherEvent
{
    public override string EventName { get; } = "None";
    public override string Description { get; set; } = "No active weather events.";
}
