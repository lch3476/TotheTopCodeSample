using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{

    // Movement Related
    public static void SetVelocity(in Rigidbody2D _rigidbodyRef, float x, float y)
    {
        Vector2 newVelocity = new Vector2(x, y);
        _rigidbodyRef.velocity = newVelocity;
    }

    public static bool IsMoving(in Rigidbody2D _rigidbodyRef)
    {
        return Mathf.Abs(_rigidbodyRef.velocity.x) > Mathf.Epsilon;
    }

    public static void SetGravity(in Rigidbody2D _rigidbodyRef, float _scale)
    {
        _rigidbodyRef.gravityScale = _scale;
    }

    public static void SetSpriteLocalScaleX(SpriteRenderer _spriteRendererRef,float _xScale)
    {
        _spriteRendererRef.transform.localScale = new Vector2(_xScale, 1f);
    }

    public static void GameObjectHorizontalMove(Vector2 _destination, string _objectName, float _speed)
    {
        GameObject target = GameObject.Find(_objectName);
        
        if (target)
        {
            target.transform.Translate(new Vector3(Mathf.Sign(_destination.x - target.transform.position.x) * _speed * Time.deltaTime, 0.0f, 0.0f));
        }
    }



    // Ray
    public static RaycastHit2D RaycastToLocalScaleX(SpriteRenderer _spriteRendererRef, float _distance, float _xOffset = 0.0f, float _yOffset = 0.0f)
    {
        RaycastHit2D result;
        if (_spriteRendererRef.transform.localScale.x >= 1.0f)
        {
            result = Physics2D.Raycast(_spriteRendererRef.gameObject.transform.position + new Vector3(_xOffset, -_yOffset, 0.0f),
            Vector2.right,
            _distance);
        }
        else
        {
            result = Physics2D.Raycast(_spriteRendererRef.gameObject.transform.position + new Vector3(-_xOffset, -_yOffset, 0.0f),
            Vector2.left,
            _distance);
        }

        return result;
    }

    public static void BidirectionalHorizontalDrawLine(Vector3 _origin, float _distance, float _xOffset = 0.0f, float _yOffset = 0.0f)
    {
        Debug.DrawLine(_origin + new Vector3(_xOffset, -_yOffset, 0.0f),
        _origin + new Vector3(_xOffset + _distance, -_yOffset, 0.0f),
        Color.red);

        Debug.DrawLine(_origin + new Vector3(-_xOffset, -_yOffset, 0.0f),
        _origin + new Vector3(-_xOffset - _distance, -_yOffset, 0.0f),
        Color.red);
    }
}
