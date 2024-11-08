using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slidable : MonoBehaviour
{

    Vector2 startPosition;
    Vector2 endPosition;
    
    [SerializeField] AnimationCurve slowdownCurve;
    [SerializeField] float distance;
    [SerializeField] float slideDuration;

    Vector2 velocity;
    float elapsedTime;
    bool slideSwitch;

    Sensor sensor1;
    // sensor2 is for a player
    Sensor sensor2;
    
    public bool SlideSwitch { get { return slideSwitch; }}

    private void Update() {
        Slide();
    }

    private void Slide()
    {
        if (slideSwitch)
        {
            if (elapsedTime <= slideDuration)
            {
                elapsedTime += Time.deltaTime;
                if (!IsTouchingWall())
                {
                    float t = Mathf.Clamp01(elapsedTime);
                    float slowdownFactor = slowdownCurve.Evaluate(t);
                    Vector2 displacement = velocity * Time.deltaTime;
                    velocity *= slowdownFactor;
                    gameObject.transform.position += (Vector3)displacement;
                }
            }
            else
            {
                InitializeSlide();
            }
        }
    }

    private void InitializeSlide()
    {
        slideSwitch = false;
        elapsedTime = 0.0f;
    }

    public void StartSlide(in SpriteRenderer _spriteRenderer, in Animator _animator = null, in string _triggerName = "")
    {
        if (!slideSwitch)
        {
            slideSwitch = true;
            startPosition = transform.position;
            endPosition = startPosition + new Vector2(distance * _spriteRenderer.transform.localScale.x, 0.0f);
            velocity = (endPosition - startPosition) / slideDuration;

            if (_animator)
            {
                _animator.SetTrigger(_triggerName);
            }
        }
    }

    // Has to be called at slidable objects constructors
    public void SetSensors(in Sensor _sensor1, in Sensor _sensor2 = null)
    {
        sensor1 = _sensor1;
        sensor2 = _sensor2;
    }

    private bool IsTouchingWall()
    {
        if (sensor1 && sensor2)
        {
            return (sensor1.State() || sensor2.State());
        }
        
        if (sensor1)
        {
            return sensor1.State();
        }

        return false;
    }
}
