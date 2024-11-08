using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    int collisionCount = 0;
    float disableTimer = 0f;

    void Update()
    {
        if (disableTimer > Mathf.Epsilon)
        {
            disableTimer -= Time.deltaTime;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag != "Enemy" && other.gameObject.tag != "Player")
        {
            collisionCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag != "Enemy" && other.gameObject.tag != "Player")
        {            
            collisionCount--;
        }
    }

    public bool State()
    {
        if (disableTimer > Mathf.Epsilon) return false;

        return collisionCount > 0;
    }

    public void Disable(float duration)
    {
        disableTimer = duration;
    }
}
