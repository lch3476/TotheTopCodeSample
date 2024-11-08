using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Chatacter Health Stat", fileName = "new CharacterHealthStatSO")]
public class CharacterHealthStatSO : ScriptableObject
{
    [SerializeField] float maxHP;

    public float MaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }
}
