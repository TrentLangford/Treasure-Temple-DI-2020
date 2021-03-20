using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : Interactable
{
    // The interactable class for all the quest items.

    // an id is required to identify the quest items.
    public int id;

    // All interactables must implement this method.
    public void TriggerOnInteractMethod(PlayerScript ps)
    {
        OnInteractWith(ps);
    }
    public override void OnInteractWith(PlayerScript ps)
    {
        // Standard interactable code, adds the item to your inventory if there is an available space.
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
