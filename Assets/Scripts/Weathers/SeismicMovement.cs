using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SeismicMovement : WeatherEvent
{
    public override string EventName { get; }= "Seismic Movement";
    public override string Description { get; } = "Only air and earth can jump";
    
    private GameObject _player;
    private SlimeController _slime;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _slime = _player.GetComponent<SlimeController>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (!(_slime.element == Element.Air || _slime.element == Element.Earth))
        {
            _player.GetComponent<Jump>().enabled = false;
        }
        else
        {
            _player.GetComponent<Jump>().enabled = true;
        }
    }
}
