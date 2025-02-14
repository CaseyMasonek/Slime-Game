using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour, IMovementController
{
    [SerializeField] private float speed;
    public float GetMovement()
    {
        return speed;
    }
}
