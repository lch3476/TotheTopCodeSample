using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using Item;

public class SkeletonCombat : EnemyCombatBase
{

    private void Awake() {
        Initialize();
    }

    public override void MeleeAttack()
    {
        if (CanAct())
        {
            animatorRef.SetTrigger("meleeAttack");
            isAttacking = true;
        }
    }

    public override void OnHurt()
    {
        animatorRef.SetTrigger("Hurt");
        isTakingDamage = true;
    }

    public override void Death(float _destroyDelayTime)
    {
        if (!isDead)
        {
            isDead = true;
            OnDeath();

            Destroy(this.gameObject, _destroyDelayTime);
        }
    }

    public override void OnDeath()
    {
        if (damagableRef.IsHPLessThan(0.0f))
        {
            animatorRef.SetTrigger("Death");
            InstantiateLoots();
        }

    }

    void InstantiateLoots()
    {
        if (DropItemList.Count > 0)
        {
            foreach (ItemBase item in DropItemList)
            {
                Instantiate(
                    item,
                    this.gameObject.transform.position + new Vector3(0.0f, 2.0f, 0.0f),
                    this.gameObject.transform.rotation
                    );
            }
        }
    }
}
