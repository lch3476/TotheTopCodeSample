using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Item;
public abstract class EnemyCombatBase : MonoBehaviour
{
    protected Animator animatorRef;
    protected Damagable damagableRef;

    // Statesj
    protected bool isAttacking;
    protected bool isTakingDamage;
    protected bool isDead;

    [SerializeField] protected List<ItemBase> DropItemList;


    public abstract void OnHurt();
    public abstract void MeleeAttack();
    public abstract void Death(float _destroyDelayTime);
    public abstract void OnDeath();

    public void Initialize()
    {
        animatorRef = GetComponent<Animator>();
        damagableRef = GetComponent<Damagable>();
    }
    
    // To prevent enemy from acting while taking damage
    public void SetIsAttackingFalse()
    {
        isAttacking = false;
    }

    // To prevent enemy from acting while taking damage
    public void SetIsTakingDamageFalse()
    {
        isTakingDamage = false;
    }

    public bool CanAct()
    {
        return !isAttacking && !isTakingDamage && !isDead;
    }
}
