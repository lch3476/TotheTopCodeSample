using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    Damagable damagableRef;


    bool canComboAttack = false;
    int comboCount = 0;

    // SerializeFields
    [Header("Combet Properties")]
    [SerializeField] float comboAttackDamage = 25f;
    [SerializeField] float horizontalAttackRange = 2.4f;
    [SerializeField] float verticalAttackRange = 1.25f;
    [SerializeField] float landingAttackLadingSpeed = 20f;

    // Offsets
    [Header("Offsets")]
    [SerializeField] float comboAttackXOffset = 0.7f;
    [SerializeField] float comboAttackYOffset = 0.9f;

    //SFX
    [Header("Sound Effects")]
    [SerializeField] AudioClip landingAttckSFX;
    [SerializeField] AudioClip parrySFX;
    [SerializeField] AudioClip swordAttackSFX; 
    [SerializeField] AudioClip deathSFX;

    private void Awake() {
        damagableRef = GetComponent<Damagable>();
    }

    private void Update() {
        OnDeath();
    }

    /************** Combat System **************/
    void OnAttack(InputValue value)
    {
        Attack();
    }

    private void Attack()
    {
        if (Player.Instance.CanAttack())
        {
            if (Player.Instance.SensorGround.State())
            {
                MeleeAttack();
            }
            else if (!Player.Instance.SensorGround.State() 
            && !Player.Instance.IsAttacking 
            && !Input.GetKey(KeyCode.S))
            {
                AirAttack();
            }
            else if (!Player.Instance.SensorGround.State() 
            && !Player.Instance.IsAttacking 
            && Input.GetKey(KeyCode.S))
            {
                LandingAttack();
            }
        }
    }

    private void LandingAttack()
    {
        Player.Instance.IsAttacking  = true;
        Utility.SetVelocity(Player.Instance.RigidbodyRef, 0f, -landingAttackLadingSpeed);
        AudioManager.instance.PlaySoundClipAtMainCamera(landingAttckSFX);
        Player.Instance.AnimatorRef.SetTrigger("LandingAttack");
    }

    private void AirAttack()
    {
        Player.Instance.IsAttacking  = true;
        Player.Instance.AnimatorRef.SetTrigger("airAttack");
        AudioManager.instance.PlaySoundClipAtMainCamera(swordAttackSFX);
    }

    private void MeleeAttack()
    {
        if (!Player.Instance.IsAttacking && (comboCount == 0))
        {
            AudioManager.instance.PlaySoundClipAtMainCamera(swordAttackSFX);
            Player.Instance.AnimatorRef.SetTrigger("attack1Trigger");
            Player.Instance.IsAttacking  = true;
        }
        else if (Player.Instance.IsAttacking  && canComboAttack && (comboCount == 1))
        {
            AudioManager.instance.PlaySoundClipAtMainCamera(swordAttackSFX);
            Player.Instance.AnimatorRef.SetTrigger("attack2Trigger");
        }
        else if (Player.Instance.IsAttacking  && canComboAttack && (comboCount == 2))
        {
            AudioManager.instance.PlaySoundClipAtMainCamera(swordAttackSFX);
            Player.Instance.AnimatorRef.SetTrigger("attack3Trigger");
        }
    }

    void OnParry(InputValue value)
    {
        if (Player.Instance.CanJumpSlideParry() 
        && Player.Instance.SensorGround.State())
        {
            Player.Instance.IsParrying = true;
            AudioManager.instance.PlaySoundClipAtMainCamera(parrySFX);
            Player.Instance.AnimatorRef.SetTrigger("ParryTrigger");
        }
    }

    void OnDeath()
    {
        if (damagableRef.IsHPLessThan(0.0f) && !Player.Instance.IsDead)
        {
            AudioManager.instance.PlaySoundClipAtMainCamera(deathSFX);
            Player.Instance.AnimatorRef.SetTrigger("Death");
            Player.Instance.RigidbodyRef.velocity = new Vector2(0.0f, Player.Instance.RigidbodyRef.velocity.y);
            Player.Instance.IsDead = true;
            Player.Instance.LevelManagerRef.Instance.DelayedLoadScene("Level1", 3.0f);
        }
    }

    /************** Damage Application **************/
    public void ApplyComboAttackDamage()
    {
        RaycastHit2D hitResult;

        float attackRange;

        if (comboCount == 2)
            attackRange = verticalAttackRange;
        else
            attackRange = horizontalAttackRange;

        hitResult = Utility.RaycastToLocalScaleX(Player.Instance.SpriteRendererRef, attackRange, comboAttackXOffset, comboAttackYOffset);
        
        if (hitResult && hitResult.collider.gameObject.tag == "Enemy")
        {
            hitResult.collider.gameObject.GetComponent<Damagable>().DirectDamage(comboAttackDamage, hitResult);
        }
    }


    /************** Anim Events **************/
    public void InitializeCombo()
    {
        Player.Instance.IsAttacking  = false;
        comboCount = 0;
        canComboAttack = false;
    }

    public void IncreaseComboCount()
    {
        comboCount++;
        canComboAttack = true;
    }

    public void SetCanComboAttackFalse()
    {
        canComboAttack = false;
    }

    public void SetIsAttackingFalse()
    {
        Player.Instance.IsAttacking = false;
    }
}
