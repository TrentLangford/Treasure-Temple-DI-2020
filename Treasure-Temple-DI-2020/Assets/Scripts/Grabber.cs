using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : Interactable
{
    // A small class responsible for the grabber in world space, that is,
    // when the grabber is in the world but not in a player's inventory

    // All interactables must implement his method.
    public void TriggerOnInteractMethod(PlayerScript ps)
    {
        OnInteractWith(ps);
    }
    public override void OnInteractWith(PlayerScript ps)
    {
        // Standard interactable method. adds this item to your inventory if there is an available spot.
        for (int i = 0; i < ps.inventorySize; i++)
        {
            if (ps.isFull[i] == false)
            {
                for (int l = 0; l < ps.inventorySize; l++) if (ps.inventory[i] == this.gameObject) break;
                ps.inventory[i] = this.gameObject;
                ps.isFull[i] = true;
                this.gameObject.SetActive(false);
                break;
            }
        }
        //base.OnInteractWith(ps);
    }
}
