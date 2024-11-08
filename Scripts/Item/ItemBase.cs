using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public abstract class ItemBase : MonoBehaviour
    {
        [SerializeField] AudioClip dropSFX;
        [SerializeField] AudioClip pickUpSFX;

        void Start() 
        {
            OnDropped();
        }

        public void OnDropped()
        {
            AudioManager.instance.PlaySoundClipAtMainCamera(dropSFX);

        }

        public void OnPickedUp()
        {
            AudioManager.instance.PlaySoundClipAtMainCamera(pickUpSFX);
            Destroy(this.gameObject);
        }
    }
}
