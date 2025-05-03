using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Typhoon : WeatherEvent
{
    public override string EventName { get; } = "Typhoon";
    public override string Description { get; set; } = "LEFT horizontal movement debuffed (changes randomly)";
    
    [SerializeField] private float debuff = 0.25f;
    
    // Start is called before the first frame update
    private GameObject _player;
    private SlimeController _slime;
    
    private int _windDirection;
    private Direction _playerDirection;
    
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _slime = _player.GetComponent<SlimeController>();
        _playerDirection = _player.GetComponent<Direction>();
        _windDirection = Random.Range(0, 2) * 2 - 1;

        StartCoroutine(Swap());
    }

    // Update is called once per frame
    void Update()
    {
        if (_windDirection == 1)
        {
            Description = "RIGHT horizontal movement debuffed (changes randomly)";
        }
        else if (_windDirection == -1)
        {
            Description = "LEFT horizontal movement debuffed (changes randomly)";
        }
        
        if (_playerDirection.AsSign() == _windDirection)
        {
            _slime.movementScale = debuff;
        }
        else
        {
            _slime.movementScale = 1;
        }
    }

    private IEnumerator Swap()
    {
        yield return new WaitForSeconds(Random.Range(3, 8));
        _windDirection *= -1;
        StartCoroutine(Swap());
    }

    private void OnDestroy()
    {
        _slime.movementScale = 1;
    }
}
