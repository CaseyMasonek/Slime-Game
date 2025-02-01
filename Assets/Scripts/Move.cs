using UnityEngine;

[RequireComponent(typeof(IMovementController), typeof(Rigidbody2D))]
public class Move : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float _maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float _maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float _maxAirAcceleration = 20f;
    [SerializeField, Range(0f, 100f)] private float _maxDeceleration = 50f;
    [SerializeField, Range(0f, 100f)] private float _maxAirDeceleration = 40f;

    private IMovementController _controller;
    private Vector2 _desiredVelocity;
    private Rigidbody2D _body;
    private Ground _ground;
    private Direction _direction;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _ground = GetComponent<Ground>();
        _controller = GetComponent<IMovementController>();
        _direction = GetComponent<Direction>();
    }

    public void SetMaxSpeed(float speed)
    {
        _maxSpeed = speed;
    }

    private void Update()
    {
        float xMovement = _controller.GetMovement();
        _direction?.SetBySign(xMovement);
        _desiredVelocity = new Vector2(xMovement, 0f) * Mathf.Max(_maxSpeed - (_ground?.friction ?? 0), 0f);
        if (_ground != null && _ground.onGround)
        {
            Rigidbody2D groundBody = _ground.ground.GetComponent<Rigidbody2D>();
            if (groundBody)
            {
                _desiredVelocity += groundBody.velocity;
            }
        }
    }

    private void FixedUpdate()
    {
        float acceleration;
        if (_controller.GetMovement() == 0)
        {
            acceleration = _ground?.onGround ?? false ? _maxDeceleration : _maxAirDeceleration;
        }
        else
        {
            acceleration = _ground?.onGround ?? false ? _maxAcceleration : _maxAirAcceleration;
        }
        float xVelocity = Mathf.MoveTowards(_body.velocity.x, _desiredVelocity.x, acceleration * Time.fixedDeltaTime);
        _body.velocity = new Vector2(xVelocity, _body.velocity.y);
    }
}
