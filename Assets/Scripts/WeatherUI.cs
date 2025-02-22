using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeatherUI : MonoBehaviour
{
    private WeatherEvent _weatherEvent;
    private GameObject _nameUI;
    private GameObject _descriptionUI;

    private void Start()
    {
        _weatherEvent = GetComponent<WeatherEvent>();
        _nameUI = GameObject.Find("Weather name").gameObject;
        _descriptionUI = GameObject.Find("Weather description").gameObject;
    }

    private void Update()
    {
        _weatherEvent = GetComponent<WeatherEvent>();
        
        if (_weatherEvent == null)
        { 
            _nameUI.GetComponent<TextMeshProUGUI>().text = "";
            _descriptionUI.GetComponent<TextMeshProUGUI>().text = "";
            return;
        };
        
        _nameUI.GetComponent<TextMeshProUGUI>().text = _weatherEvent.EventName;
        _descriptionUI.GetComponent<TextMeshProUGUI>().text = _weatherEvent.Description;
    }
}
