using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utility;

public class PlayerAnimEventsManager : MonoBehaviour
{
    PlayerMovement playerMovementRef;
    PlayerCombat playerCombatRef;
    Rigidbody2D rigidbodyRef;

    void Awake() {

        playerMovementRef = GetComponentInParent<PlayerMovement>();
        playerCombatRef = GetComponentInParent<PlayerCombat>();

        rigidbodyRef = GetComponentInParent<Rigidbody2D>();
    }

    // Set States
    public void SetClimbPosition()
    {
        playerMovementRef.SetClimbPosition();
    }

    public void SetIsClimbingLedgeTrue()
    {
        Player.Instance.IsClimbingLedge = true;
    }

    public void SetIsDodgingFalse()
    {
        Player.Instance.IsDodging = false;
    }
    
    public void SetIsAttackingFalse()
    {
        Player.Instance.IsAttacking =false;
    }
    
    public void SetIsParryingFalse()
    {
        Player.Instance.IsParrying = false;
    }

    public void SetIsTakingDamageFalse()
    {
        Player.Instance.IsTakingDamage = false;
    }

    public void SetIsTakingDamageTrue()
    {
        Player.Instance.IsTakingDamage = true;
    }

    public void ClearState()
    {
        Player.Instance.ClearState();
    }
    
    // Set Values
    public void SetSpeedtoRunSpeed()
    {
        playerMovementRef.SetCharaterSpeedToDefault();
    }

    public void SetGravity(float _scale)
    {
        Utility.SetGravity(rigidbodyRef, _scale);
    }

    public void SetGravityToDefault()
    {
        Utility.SetGravity(rigidbodyRef, Player.Instance.DefaultGravity);
    }

    // Combo Events
    public void InitializeCombo()
    {
        playerCombatRef.InitializeCombo();
    }

    public void IncreaseComboCount()
    {
        playerCombatRef.IncreaseComboCount();
    }

    public void SetCanComboAttackFalse()
    {
        playerCombatRef.SetCanComboAttackFalse();
    }

    // Apply Damage Events
    public void ApplyComboAttackDamage()
    {
        playerCombatRef.ApplyComboAttackDamage();
    }

    public void StopCharacterMovement()
    {
        playerMovementRef.StopCharacterMovement();
    }
}
