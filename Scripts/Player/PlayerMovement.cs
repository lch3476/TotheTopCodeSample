using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Utility;


public class PlayerMovement : MonoBehaviour
{
    //Properties
    [Header("Movement Properties")]
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float jumpForce = 15f;
    [SerializeField] float wallSlideGravity = 2f;

    //SFXs
    [Header("Sound Effects")]
    [SerializeField] AudioClip[] runSFX;
    [SerializeField] AudioClip jumpSFX;
    [SerializeField] AudioClip dodgeSFX;
    [SerializeField] AudioClip wallSlidingSFX;

    //VFXx
    [Header("Visual Effects")]
    [SerializeField] GameObject dodgeDust;
    [SerializeField] GameObject jumpDust;
    [SerializeField] GameObject landingDust;
    [SerializeField] GameObject runStopDust;
    [SerializeField] GameObject wallJumpDust;
    [SerializeField] GameObject wallSlidDust;

    //Others
    Vector2 rawMoveInput;
    GrabableLedge holdingLedge;
    Vector3 ledgeClimbPosition;

    Slidable slidableRef;
    CharacterSFXManager characterSFXManager;

    private void Awake() {

        slidableRef = GetComponent<Slidable>();
        characterSFXManager = GetComponent<CharacterSFXManager>();
    }

    private void Start() {
        if (slidableRef)
        {
            slidableRef.SetSensors(Player.Instance.SensorLeftDown, Player.Instance.SensorRightDown);
        }
    }

    void Update()
    {
        Run();
        GrabLedge();
        ClimbOrReleaseLedge();
        SlidingWall();
        SetSpriteOrientationToMovement();

    }


    /************** Movements **************/
    // Player Input Events
    void OnMove(InputValue value)
    {
        rawMoveInput = value.Get<Vector2>();
    }
    

    void OnJump(InputValue value)
    {
        Jump();
    }

    void OnDodge(InputValue value)
    {
        Dodge();
    }

    // Movement methods
    void Run()
    {
        if (Player.Instance.CanAct())
        {
            Utility.SetVelocity(Player.Instance.RigidbodyRef, Mathf.Round(rawMoveInput.x) * runSpeed, Player.Instance.RigidbodyRef.velocity.y);
            if (Utility.IsMoving(Player.Instance.RigidbodyRef))
            {
                Player.Instance.AnimatorRef.SetBool("isRunning", true);

                if (Player.Instance.SensorGround.State())
                {
                    characterSFXManager.PlayClipsRandom(runSFX);
                }
            }
            else
            {
                Player.Instance.AnimatorRef.SetBool("isRunning", false);
            }
        }
    }


    void Jump()
    {
        if (Player.Instance.CanJumpSlideParry())
        {    

            if (Player.Instance.SensorGround.State() || Player.Instance.IsSlidingWall)
            {
                Utility.SetVelocity(Player.Instance.RigidbodyRef, Player.Instance.RigidbodyRef.velocity.x, jumpForce);
                Player.Instance.AnimatorRef.SetTrigger("jumpTrigger");
                AudioManager.instance.PlaySoundClipAtMainCamera(jumpSFX);
                
            }
        }
    }


    void Dodge()
    {
        if (Player.Instance.CanAct()
        && Player.Instance.SensorGround.State() 
        && Utility.IsMoving(Player.Instance.RigidbodyRef))
        {
            slidableRef.StartSlide(Player.Instance.SpriteRendererRef, Player.Instance.AnimatorRef, "dodgeTrigger");
            AudioManager.instance.PlaySoundClipAtMainCamera(dodgeSFX);
            Player.Instance.EffectManagerRef.InstantiateEffect(dodgeDust, Player.Instance.SensorGround.gameObject.transform);
            Player.Instance.IsDodging = slidableRef.SlideSwitch;
        }
    }



    void SlidingWall()
    {
        if (Player.Instance.SensorLeftDown && Player.Instance.SensorLeftUp && Player.Instance.SensorRightDown && Player.Instance.SensorRightUp)
        {

            if (Player.Instance.CanJumpSlideParry()
            && Player.Instance.IsFalling() 
            && !Player.Instance.SensorGround.State() 
            && ((Player.Instance.SensorLeftDown.State() && Player.Instance.SensorLeftUp.State() && Input.GetKey(KeyCode.A)) || (Player.Instance.SensorRightDown.State() && Player.Instance.SensorRightUp.State() && Input.GetKey(KeyCode.D)))
            )
            {
                Player.Instance.IsSlidingWall = true;
                Player.Instance.RigidbodyRef.gravityScale = wallSlideGravity;
                characterSFXManager.PlaySingleClip(wallSlidingSFX);
            }
            else
            {
                if (Player.Instance.IsSlidingWall)
                {
                    Utility.SetSpriteLocalScaleX(Player.Instance.SpriteRendererRef, -Mathf.Sign(Player.Instance.SpriteRendererRef.transform.localScale.x));
                    Player.Instance.DisableSensors(1f, 1f, -Player.Instance.GetCharacterFacingDirection());
                    Player.Instance.RigidbodyRef.gravityScale = Player.Instance.DefaultGravity;
                    Player.Instance.IsSlidingWall = false;
                    characterSFXManager.StopPlaying();
                }
            }

            Player.Instance.AnimatorRef.SetBool("isSlidingWall", Player.Instance.IsSlidingWall);
        }
    }


    void GrabLedge()
    {
        if (Player.Instance.SensorLeftDown && Player.Instance.SensorLeftUp && Player.Instance.SensorRightDown && Player.Instance.SensorRightUp)
        {
            if (Player.Instance.CanAct()
            && Player.Instance.IsFalling() 
            && !Player.Instance.SensorGround.State() 
            && ((!Player.Instance.SensorLeftUp.State() && Player.Instance.SensorLeftDown.State()) || (!Player.Instance.SensorRightUp.State() && Player.Instance.SensorRightDown.State())))
            {
                Vector2 rayStart;

                if (Player.Instance.GetCharacterFacingDirection() == 1)
                    rayStart = Player.Instance.SensorRightUp.transform.position + new Vector3(0.2f, 0.0f, 0.0f);
                else
                    rayStart = Player.Instance.SensorLeftUp.transform.position - new Vector3(0.2f, 0.0f, 0.0f);

                var hitResult = Physics2D.Raycast(rayStart, Vector2.down, 1.0f);
                
                if (hitResult)
                {
                    holdingLedge = hitResult.transform.GetComponent<GrabableLedge>();
                }

                if (holdingLedge)
                {
                    Player.Instance.RigidbodyRef.velocity = Vector2.zero;
                    Player.Instance.RigidbodyRef.gravityScale = 0;
                    
                    if (Player.Instance.GetCharacterFacingDirection() == -1)
                        transform.position = holdingLedge.transform.position + new Vector3(holdingLedge.GetGrabPosition().x, holdingLedge.GetGrabPosition().y, 0.0f);
                    else
                        transform.position = holdingLedge.transform.position + new Vector3(-holdingLedge.GetGrabPosition().x, holdingLedge.GetGrabPosition().y, 0.0f);
                    Player.Instance.IsGrabbingLedge  = true;
                }
            }
        
            Player.Instance.AnimatorRef.SetBool("isGrabbingledge", Player.Instance.IsGrabbingLedge);
        }
    }
        
    void ClimbOrReleaseLedge()
    {
        if (Player.Instance.IsGrabbingLedge 
        && (((Player.Instance.GetCharacterFacingDirection() == 1 && Input.GetKeyDown(KeyCode.A)) || (Player.Instance.GetCharacterFacingDirection() == -1 && Input.GetKeyDown(KeyCode.D)))
        || Input.GetKeyDown(KeyCode.S))
        )
        {
            Player.Instance.IsGrabbingLedge = false;
            Player.Instance.RigidbodyRef.gravityScale = Player.Instance.DefaultGravity;
            holdingLedge = null;
            Player.Instance.AnimatorRef.SetBool("isGrabbingledge", Player.Instance.IsGrabbingLedge);
        }
        else if (Player.Instance.IsGrabbingLedge && Input.GetKeyDown(KeyCode.W))
        {
            Player.Instance.IsClimbingLedge = true;
            Player.Instance.IsGrabbingLedge = false;
            Player.Instance.AnimatorRef.SetBool("isGrabbingledge", Player.Instance.IsGrabbingLedge);
            if (Player.Instance.GetCharacterFacingDirection() == -1)
            {
                ledgeClimbPosition = holdingLedge.transform.position + new Vector3(-holdingLedge.GetClimbPosition().x, holdingLedge.GetClimbPosition().y, 0.0f);
            }
            else
            {
                ledgeClimbPosition = holdingLedge.transform.position + new Vector3(holdingLedge.GetClimbPosition().x, holdingLedge.GetClimbPosition().y, 0.0f);
            }
            Player.Instance.AnimatorRef.SetBool("isClimbingLedge", Player.Instance.IsClimbingLedge);
        }
    }

    public void SetClimbPosition()
    {
        // called by anim events
        transform.position = ledgeClimbPosition;
        Player.Instance.RigidbodyRef.gravityScale = Player.Instance.DefaultGravity;
        Player.Instance.IsClimbingLedge = false;
        holdingLedge = null;
        Player.Instance.AnimatorRef.SetBool("isClimbingLedge", Player.Instance.IsClimbingLedge);
    }

    /************** Sprites Related **************/
    void SetSpriteOrientationToMovement()
    {
        if (Utility.IsMoving(Player.Instance.RigidbodyRef) && Player.Instance.CanAct())
        {
            Utility.SetSpriteLocalScaleX(Player.Instance.SpriteRendererRef, Mathf.Sign(Player.Instance.RigidbodyRef.velocity.x));
        }

    }

    /************** Anim Events **************/

    public void SetCharaterSpeedToDefault()
    {
        if (Player.Instance.GetCharacterFacingDirection() == 1)
            Utility.SetVelocity(Player.Instance.RigidbodyRef, runSpeed, Player.Instance.RigidbodyRef.velocity.y);
        else if (Player.Instance.GetCharacterFacingDirection() == -1)
            Utility.SetVelocity(Player.Instance.RigidbodyRef, -runSpeed, Player.Instance.RigidbodyRef.velocity.y);
    }

    public void StopCharacterMovement()
    {
        Utility.SetVelocity(Player.Instance.RigidbodyRef, 0f, Player.Instance.RigidbodyRef.velocity.y);
    }

    public void SetGravity(float _scale)
    {
        Utility.SetGravity(Player.Instance.RigidbodyRef, _scale);
    }
}
