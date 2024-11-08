using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Status", fileName = "new EnemySO")]
public class EnemySO : ScriptableObject
{
    [SerializeField] float maxHP;
    [SerializeField] float currentHP;
    [SerializeField] float moveSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float detectRange;
    [SerializeField] float chaseRange;
    [SerializeField] float attackDamage;
    [SerializeField] float attackRange;

    public float MaxHP 
    { 
        get { return maxHP; } 
        set { maxHP = value; }
    }

    public float CurrentHP
    {
        get { return currentHP; }
        set { currentHP = value; }     
    }

    public float MoveSpeed 
    {
        get { return moveSpeed; } 
        set { moveSpeed = value; }
    }

    public float RunSpeed 
    { 
        get { return runSpeed; } 
        set { runSpeed = value; } 
    }
    
    public float DetectRange
    {
        get { return detectRange; }
        set { detectRange = value; }
    }

    public float ChaseRange
    {
        get { return chaseRange; }
        set { chaseRange = value; }
    }

    public float AttackDamage
    {
        get { return attackDamage; }
        set { attackDamage = value; }
    }

    public float AttackRange
    {
        get { return attackRange; }
        set { attackRange = value; }
    }
}
