using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeWeather : MonoBehaviour
{
    [SerializeField] private WeatherEvent weatherEvent;

    private GameObject _weather;

    private void Start()
    {
        _weather = GameObject.FindGameObjectWithTag("Weather");
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(_weather.GetComponent<WeatherEvent>());

            if (weatherEvent != null)
            {
                _weather.AddComponent(weatherEvent.GetType());
            }
        }
    }
}
