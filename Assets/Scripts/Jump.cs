using UnityEngine;

[RequireComponent(typeof(IJumpController), typeof(Ground), typeof(Rigidbody2D))]
public class Jump : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)] private float _jumpHeight = 3f;
    [SerializeField, Range(0, 5)] private int _maxAirJumps = 0;
    [SerializeField, Range(0f, 5f)] private float _downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float _upwardMovementMultiplier = 1.7f;

    private IJumpController _controller;
    private Rigidbody2D _body;
    private Ground _ground;
    private int _jumpPhase;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _ground = GetComponent<Ground>();
        _controller = GetComponent<IJumpController>();
    }

    private void OnEnable()
    {
        _controller.OnJump += JumpAction;
    }

    private void OnDisable()
    {
        _controller.OnJump -= JumpAction;
    }

    private void FixedUpdate()
    {
        if (_ground.onGround)
        {
            _jumpPhase = 0;
        }

        if (_body.velocity.y > 0)
        {
            _body.gravityScale = _upwardMovementMultiplier;
        }
        else if (_body.velocity.y < 0)
        {
            _body.gravityScale = _downwardMovementMultiplier;
        }
        else if (_body.velocity.y == 0)
        {
            _body.gravityScale = 1f;
        }
    }

    private void JumpAction()
    {
        if (!_ground.onGround && _jumpPhase >= _maxAirJumps) return;
        _jumpPhase += 1;

        float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _jumpHeight);
        if (_body.velocity.y > 0f)
        {
            jumpSpeed = Mathf.Max(jumpSpeed - _body.velocity.y, 0f);
        }
        else if (_body.velocity.y < 0f)
        {
            jumpSpeed += Mathf.Abs(_body.velocity.y);
        }
        _body.velocity += Vector2.up * jumpSpeed;
    }
}