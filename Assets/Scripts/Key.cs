using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactable
{
    public int id;
    public void TriggerOnInteractMethod(PlayerScript ps)
    {
        OnInteractWith(ps);
    }

    public override void OnInteractWith(PlayerScript ps)
    {
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
