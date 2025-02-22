using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SeismicMovement : MonoBehaviour
{
    //public string eventName = "Seismic Movement";
    //public string description = "Only air and earth can jump";
    
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
        if (!(_slime.element == SlimeController.Element.Air || _slime.element == SlimeController.Element.Earth))
        {
            _player.GetComponent<Jump>().enabled = false;
        }
        else
        {
            _player.GetComponent<Jump>().enabled = true;
        }
    }
}
