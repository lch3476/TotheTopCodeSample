using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Item;

public class Coin : ItemBase
{
    enum Type { COPPER, SILVER, GOLD }

    [SerializeField] Type type;
    Animator animatorRef;

    int amount;
    bool isSpinning;

    public int Amount 
    { get { return amount; }
      set { amount = value; }
    }

    void Awake() {
        SetAmountRandomByType();
        animatorRef = GetComponent<Animator>();
    }

    void Update()
    {
        Spin();
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            base.OnPickedUp();
        }   
    }

    private void Spin()
    {
        if (!isSpinning)
        {
            switch (type)
            {
                case Type.COPPER:
                    animatorRef.SetTrigger("Copper");
                    break;
                case Type.SILVER:
                    animatorRef.SetTrigger("Silver");
                    break;
                case Type.GOLD:
                    animatorRef.SetTrigger("Gold");
                    break;
            }

            isSpinning = true;
        }
    }

    void SetAmountRandomByType()
    {
        switch(type)
        {
            case Type.COPPER:
                amount = Random.Range(10, 31);
                break;
            case Type.SILVER:
                amount = Random.Range(31, 70);
                break;
            case Type.GOLD:
                amount = Random.Range(71, 100);
                break;
        }
    }

    public void SetIsSpinningFalse()
    {
        isSpinning = false;
    }
}
