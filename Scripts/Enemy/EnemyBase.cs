using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public abstract class EnemyBase : MonoBehaviour
    {
        protected enum State { REST, SEARCH, ENGAGE, DEATH};

        protected State state = State.REST;
        protected GameObject target;

        protected bool isResting;

        protected Damagable damagableRef;
        protected Animator animatorRef;
        protected SpriteRenderer spriteRendererRef;
        protected EnemyCombatBase combatRef;

        protected Vector2 currentDestination;
        protected int destinationIdx;
        protected int lastDestinationIdx;
        protected float destroyDelayTime = 3.0f;

        [SerializeField] protected Vector2[] navPoints;
        [SerializeField] protected EnemySO statSO;
        [SerializeField] protected float attackXOffset;
        [SerializeField] protected float attackYOffset;

        [SerializeField] protected float detectionStartXOffset;
        [SerializeField] protected float detectionStartYOffset;

        // Behaviors
        public virtual void Behavior()
        {
            switch (state)
            {
                case State.REST:
                    Rest();
                    break;
                case State.SEARCH:
                    Search();
                    break;
                case State.ENGAGE:
                    Engage();
                    break;
                case State.DEATH:
                    combatRef.Death(destroyDelayTime);
                    break;
                default:
                    break;
            }
        }
        
        // Helper methods
        public void IsAlive()
        {
            if (damagableRef.IsHPLessThan(0.0f))
            {
                state = State.DEATH;
            }
        }

        // Initialization
        public void Initialize()
        {
            damagableRef      = GetComponent<Damagable>();
            animatorRef       = GetComponent<Animator>();
            spriteRendererRef = GetComponentInChildren<SpriteRenderer>();
        }

        // Interfaces
        public abstract void Rest();
        public abstract void Search();
        public abstract void Engage();
        public abstract void DetectPlayer();
        public abstract void ApplyDamage();
        public abstract void Move(Vector2 _destination, float _speed);
    }
}
