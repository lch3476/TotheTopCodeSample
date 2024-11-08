using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

public class Skeleton : EnemyBase
{
    // SerailizeField
    [SerializeField] float maxRestTime = 3f;

    float distanceEpsilon = 0.1f;


    void Awake()
    {
        Initialize();
        combatRef = GetComponent<SkeletonCombat>();
    }

    void Update()
    {
        SetOrientationToMovement();
        DetectPlayer();
        SkeletonBehavior();
        IsAlive();
    }

    private void SkeletonBehavior()
    {
        Behavior();
    }

    public override void Rest()
    {
        StartCoroutine(RestAndBehave());
    }

    IEnumerator RestAndBehave()
    {   
        animatorRef.SetBool("isMoving", false);
        
        if (!isResting)
        {
            isResting = true;

            yield return new WaitForSecondsRealtime(Random.Range(0f, maxRestTime));

            isResting = false;

            state = State.SEARCH;
        }
    }
    public override void Search()
    {
        target = null;
        lastDestinationIdx = destinationIdx;

        if (IsAtDestination())
        {
            SetNextDestinationIdx();
            state = State.REST;
        }
        
        currentDestination = navPoints[destinationIdx];

        Move(currentDestination, statSO.MoveSpeed);
    }

    public override void Engage()
    {
        if (target)
        {

            float xDistanceToTarget = target.transform.position.x - transform.position.x;

            if (Mathf.Abs(xDistanceToTarget) > statSO.AttackRange && combatRef.CanAct())
            {
                if (Mathf.Sign(xDistanceToTarget) >= 1)
                {
                    currentDestination = target.transform.position - new Vector3(statSO.AttackRange, 0f, 0f);
                    Move(currentDestination, statSO.RunSpeed);
                }
                else if (Mathf.Sign(xDistanceToTarget) <= -1)
                {
                    currentDestination = target.transform.position + new Vector3(statSO.AttackRange, 0f, 0f);
                    Move(currentDestination, statSO.RunSpeed);
                }
            }
            else
            {
                animatorRef.SetBool("isMoving", false);
                combatRef.MeleeAttack();
            }

            if (Mathf.Abs(target.transform.position.x - transform.position.x) > statSO.ChaseRange)
            {
                target = null;
                state = State.REST;
            }
        }
    }

    public override void DetectPlayer()
    {
        if (!target)
        {
            RaycastHit2D hitResult = Physics2D.Raycast(
            this.transform.position + new Vector3(spriteRendererRef.transform.localScale.x * detectionStartXOffset, detectionStartYOffset, 0f), 
            spriteRendererRef.transform.localScale.x == 1 ? Vector2.right : Vector2.left, statSO.DetectRange
            );

            if (hitResult && hitResult.collider.gameObject.tag == "Player")
            {
                target = hitResult.collider.gameObject;
                state = State.ENGAGE;
            }
        }
    }

    public override void Move(Vector2 _destination, float _speed)
    {
        Utility.GameObjectHorizontalMove(_destination, "Skeleton", _speed);
        animatorRef.SetBool("isMoving", true);
    }

    /************ Helper Methods *************/
    void SetNextDestinationIdx()
    {
        do
        {
            destinationIdx = Random.Range(0, navPoints.Length);
        }
        while (lastDestinationIdx == destinationIdx);
    }

    void SetOrientationToMovement()
    {
        if (!IsAtDestination())
        {
            Utility.SetSpriteLocalScaleX(spriteRendererRef, Mathf.Sign(GetXDistanceToDestination()));
        }
    }
    
    float GetXDistanceToDestination()
    {
        return currentDestination.x - transform.position.x;
    }

    bool IsAtDestination()
    {
        return Mathf.Abs(GetXDistanceToDestination()) <= distanceEpsilon;
    }

    /************ Anim Events *************/
    public override void ApplyDamage()
    {
        RaycastHit2D hitResult;

        hitResult = Utility.RaycastToLocalScaleX(spriteRendererRef, statSO.AttackRange, attackXOffset, attackYOffset);

        if (hitResult && hitResult.collider.gameObject.tag == "Player")
        {
            hitResult.collider.gameObject.GetComponent<Damagable>().DirectDamage(statSO.AttackDamage, hitResult);
        }
    }
}
