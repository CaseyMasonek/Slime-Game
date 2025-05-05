using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
    
public class HeatWave : WeatherEvent
{
    public override string EventName { get; } = "Heat Wave";
    public override string Description { get; set;  } = "Non-fire horizontal movement debuff";
    
    [SerializeField] private float speedDebuff = 0.5f;
    
    // Start is called before the first frame update
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
        if (_slime.element != Element.Fire)
        {
            _slime.movementScale = speedDebuff; 
        }
        else
        {
            _slime.movementScale = 1;
        }
    }

    private void OnDestroy()
    {
        _slime.movementScale = 1;
    }
}
