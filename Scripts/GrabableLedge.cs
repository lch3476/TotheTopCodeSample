using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabableLedge : MonoBehaviour
{
    // These positions determine where the hero is positioned after grabbling a ledge and climbing it
    [SerializeField] Vector2 GrabPosition = new Vector2(0.55f, -0.5f);
    [SerializeField] Vector2 ClimbPosition = new Vector2(1.0f, 2.7f);

    public Vector2 GetGrabPosition()
    {
        return GrabPosition;
    }

    public Vector2 GetClimbPosition()
    {
        return ClimbPosition;
    }
}
