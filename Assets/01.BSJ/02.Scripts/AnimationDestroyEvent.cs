using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDestroyEvent : MonoBehaviour
{
    public void Destroy()
    {
        this.gameObject.SetActive(false);

        if (ItemUIController.PickupMessageObj?.Count > 3)
        {
            ItemUIController.PickupMessageObj.Remove(gameObject);
            Destroy(this.gameObject);
        }
    }
}
