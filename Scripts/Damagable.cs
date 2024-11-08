using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    [SerializeField] CharacterHealthStatSO characterHealthStatSO;
    [SerializeField] AudioClip hurtSFX;

    float currentHP;

    void Start() {
        currentHP = characterHealthStatSO.MaxHP;
    }

    public void DirectDamage(float _damage, in RaycastHit2D _hitresult)
    {
        currentHP -= _damage;
        
        if (currentHP < 0.0f)
        {
            currentHP = 0.0f;
        }

        GameObject target = _hitresult.collider.gameObject;

        Animator animatorRef = target.GetComponent<Animator>();
        
        if (!animatorRef)
        {
            animatorRef = _hitresult.collider.gameObject.GetComponentInChildren<Animator>();
        }

        if (currentHP > 0.0f)
        {
            AudioManager.instance.PlaySoundClipAtMainCamera(hurtSFX);
            animatorRef.SetTrigger("Hurt");
        }
    }

    public float CurrentHP
    {
        get { return currentHP; }
    }

    public bool IsHPLessThan(float _hp)
    {
        return currentHP <= _hp;
    }
}
