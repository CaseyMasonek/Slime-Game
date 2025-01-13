using UnityEngine;

public class Direction : MonoBehaviour
{
    private bool _isRight = true;
    private SpriteRenderer _sprite;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    public bool IsRight()
    {
        return _isRight;
    }

    public bool IsLeft()
    {
        return !_isRight;
    }

    public float AsSign()
    {
        return _isRight ? 1 : -1;
    }

    public void SetRight()
    {
        _isRight = true;
        TrySetSpriteDirection();
    }

    public void SetLeft()
    {
        _isRight = false;
        TrySetSpriteDirection();
    }

    public void SetBySign(float sign)
    {
        if (sign == 0) return;
        _isRight = sign > 0;
        TrySetSpriteDirection();
    }

    public void Flip()
    {
        _isRight = !_isRight;
        TrySetSpriteDirection();
    }

    public void TrySetSpriteDirection()
    {
        if (!_sprite) return;
        _sprite.flipX = !_isRight;
    }
} 