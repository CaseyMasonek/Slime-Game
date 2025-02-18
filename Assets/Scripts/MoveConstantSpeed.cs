using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IMovementController))]
public class MoveConstantSpeed : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.0f;
    private IMovementController _controller;

    private void Awake()
    {
        _controller = GetComponent<IMovementController>();
    }

    private void Update()
    {
        transform.Translate(_controller.GetMovement() * moveSpeed * Time.deltaTime * Vector3.right);
    }
}
