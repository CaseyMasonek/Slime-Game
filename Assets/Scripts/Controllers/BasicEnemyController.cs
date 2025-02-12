using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Direction),typeof(Ground))]
public class BasicEnemyController : MonoBehaviour, IMovementController
{
    [SerializeField] private float moveSpeed;

    private Direction _direction;
    private Ground _ground;

    private void Start()
    {
        
    }

    public float GetMovement()
    {
        return 2;
    }
}
