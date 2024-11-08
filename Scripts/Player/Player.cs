using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // Singleton
    private static Player instance;

    public static Player Instance { get { return instance; } }

    // States
    bool isSlidingWall;
    bool isGrabbingLedge;
    bool isClimbingLedge;
    bool isDodging;
    bool isAttacking;
    bool isParrying;
    bool isDead;
    bool isTakingDamage;

    // References
    Rigidbody2D rigidbodyRef;
    Animator animatorRef;
    SpriteRenderer spriteRendererRef;
    EffectManager effectManagerRef;
    
    //Sensors
    Sensor sensorGround;
    Sensor sensorRightDown;
    Sensor sensorRightUp;
    Sensor sensorLeftDown;
    Sensor sensorLeftUp;

    LevelManager levelManagerRef;

    float defaultGravity;

    // Properties
    public bool IsSlidingWall
    {
        get { return isSlidingWall; }
        set { isSlidingWall = value; }
    }

    public bool IsGrabbingLedge
    {
        get { return isGrabbingLedge; }
        set { isGrabbingLedge = value; }
    }

    public bool IsClimbingLedge
    {
        get { return isClimbingLedge; }
        set { isClimbingLedge = value; }
    }

    public bool IsDodging
    {
        get { return isDodging; }
        set { isDodging = value; }
    }

    public bool IsAttacking
    {
        get { return isAttacking; }
        set { isAttacking = value; }
    }

    public bool IsParrying
    {
        get { return isParrying; }
        set { isParrying = value; }
    }

    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }
    
    public bool IsTakingDamage
    {
        get { return isTakingDamage; }
        set { isTakingDamage = value; }
    }

    public Rigidbody2D RigidbodyRef { get { return rigidbodyRef; }}

    public Animator AnimatorRef { get { return animatorRef; }}

    public SpriteRenderer SpriteRendererRef { get { return spriteRendererRef; }}

    public Sensor SensorGround { get { return sensorGround; }}

    public Sensor SensorRightDown { get { return sensorRightDown; }}

    public Sensor SensorRightUp { get { return sensorRightUp; }}

    public Sensor SensorLeftDown { get { return sensorLeftDown; }}

    public Sensor SensorLeftUp { get { return sensorLeftUp; }}

    public float DefaultGravity { get { return defaultGravity; }}

    public LevelManager LevelManagerRef { get { return levelManagerRef; }}

    public EffectManager EffectManagerRef { get { return effectManagerRef; }}

    private void Awake() {
        
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }

        // Initialize references
        rigidbodyRef = GetComponent<Rigidbody2D>();
        animatorRef = GetComponentInChildren<Animator>();
        spriteRendererRef = GetComponentInChildren<SpriteRenderer>();
        effectManagerRef = GetComponent<EffectManager>();

        // Initialize Sensors.
        sensorGround = transform.Find("SensorGround").GetComponent<Sensor>();
        sensorLeftDown = transform.Find("SensorLD").GetComponent<Sensor>();
        sensorLeftUp = transform.Find("SensorLU").GetComponent<Sensor>();
        sensorRightDown = transform.Find("SensorRD").GetComponent<Sensor>();
        sensorRightUp = transform.Find("SensorRU").GetComponent<Sensor>();

        levelManagerRef = FindObjectOfType<LevelManager>();
    }

    private void Start() {
        defaultGravity = rigidbodyRef.gravityScale;
    }

    private void Update() {
        SetAnimatorParam();
    }

    /************** Helper Methods **************/
    public float GetCharacterFacingDirection()
    {
        return spriteRendererRef.transform.localScale.x;
    }

    public void DisableSensors(float up, float down, float direction)
    {
        // if directinon == 1, disable right sensors
        // if directinon == -1, disable left sensors
        // else disable all sensors
        if (direction == 1)
        {
            sensorRightUp.Disable(up);
            sensorRightDown.Disable(down);
        }
        else if (direction == -1)
        {
            sensorLeftUp.Disable(up);
            sensorLeftDown.Disable(down);
        }
        else
        {
            sensorRightUp.Disable(up);
            sensorRightDown.Disable(down);
            sensorLeftUp.Disable(up);
            sensorLeftDown.Disable(down);
        }
    }

    void SetAnimatorParam()
    {
        animatorRef.SetBool("isOnGround", sensorGround.State());
        animatorRef.SetBool("isFalling", IsFalling());
        animatorRef.SetBool("isDodging", isDodging);
        animatorRef.SetBool("isAttacking", isAttacking );
    }

    public bool IsFalling()
    {
        if (sensorGround.State()) 
            return false;
        else 
            return rigidbodyRef.velocity.y < Mathf.Epsilon;
    }


    public bool CanJumpSlideParry()
    {
        return !isAttacking
        && CanAttack();
    }

    public bool CanAct()
    {
        return CanJumpSlideParry()
        && !isSlidingWall;
    }

    public bool CanAttack()
    {
        return !isGrabbingLedge
        && !isClimbingLedge
        && !isDodging
        && !isParrying
        && !isDead
        && !isTakingDamage;
    }

    public void ClearState()
    {
        isSlidingWall = false;
        isGrabbingLedge = false;
        isClimbingLedge = false;
        isDodging = false;
        isAttacking = false;
        isParrying = false;
        isTakingDamage = false;
    }
}
