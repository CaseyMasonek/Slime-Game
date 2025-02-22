using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Downpour : MonoBehaviour
{
    public string eventName = "Downpour";
    public string description =  "Non-water aerial movement debuff";
    
    [SerializeField] private float debuff = 0.8f;
    
    // Start is called before the first frame update
    private GameObject _player;
    private SlimeController _slime;
    private Jump _jump;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _slime = _player.GetComponent<SlimeController>();   
        _jump = _player.GetComponent<Jump>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_slime.element != SlimeController.Element.Water)
        {
            _jump.jumpScale = debuff;
        }
        else
        {
            _jump.jumpScale = 1;
        }
    }
}
