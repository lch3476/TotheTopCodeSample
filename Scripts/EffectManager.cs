using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    GameObject currentEffect;

    public void InstantiateEffect(in GameObject effect, Transform initTransform)
    {
        currentEffect = Instantiate(effect, initTransform);
    }

    public void DestroyEffect()
    {
        Destroy(currentEffect);
    }

}
