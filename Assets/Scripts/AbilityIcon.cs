using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityIcon : MonoBehaviour
{
    [SerializeField] private string _abilityName;

    private SlimeController _slime;
    private Vector3 _initialPosition;
    private float _initialHeight;
    
    // Start is called before the first frame update
    void Start()
    {
        _slime = GameObject.FindGameObjectWithTag("Player").GetComponent<SlimeController>();
        _initialPosition = transform.position;
        _initialHeight = GetComponent<RectTransform>().sizeDelta.y;
    }

    // Update is called once per frame
    void Update()
    {
        float cooldown = (float)_slime.GetType().GetField(_abilityName).GetValue(_slime);
        transform.localScale = new Vector3(1, cooldown, 1);
        float newHeight = _initialHeight * cooldown;
        float difference = _initialHeight - newHeight;
        //transform.position = _initialPosition - new Vector3(0, difference/2, 0);
    }
}
