using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatWave : MonoBehaviour
{
    public string eventName { get; protected set; } = "Heat Wave";
    public string description = "Non-fire movement debuff";
    
    [SerializeField] private float speedDebuff = 0.8f;
    
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
        if (_slime.element != SlimeController.Element.Fire)
        {
            _slime.movementScale = speedDebuff; 
        }
        else
        {
            _slime.movementScale = 1;
        }
    }
}
