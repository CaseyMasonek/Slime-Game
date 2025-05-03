using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStorm : WeatherEvent
{
    public override string EventName { get; } = "Ice Storm";
    public override string Description { get; set; } = "All horizontal movement debuffed (especially water and air)";
    
    [SerializeField] private float normalDebuff = 0.75f;
    [SerializeField] private float extraDebuff = 0.5f;
    
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
        if (_slime.element == Element.Air || _slime.element == Element.Water)
        {
            _slime.movementScale = extraDebuff; 
        }
        else
        {
            _slime.movementScale = normalDebuff;
        }
    }

    private void OnDestroy()
    {
        _slime.movementScale = 1;
    }
}
