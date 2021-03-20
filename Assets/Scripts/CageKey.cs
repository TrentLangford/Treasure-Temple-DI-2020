using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageKey : Interactable
{
    public bool didGetSelected;
    public Rigidbody2D thisRb;
    Vector2 newPos;

    // All interactables must implement this method.
    public void TriggerOnInteractMethod(PlayerScript ps)
    {
        OnInteractWith(ps);
    }

    public override void OnInteractWith(PlayerScript ps)
    {
        // Standard interactable code. adds this item to your inventory if there is an open slot.
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

    // whenever the grabber "selects" this cage key, set the did get selected variable to true and
    // set our desired position to the player's position plus 1 unit right.
    public void Select(PlayerScript ps)
    {
        didGetSelected = true;
        newPos = new Vector2(ps.transform.position.x, ps.transform.position.y) + Vector2.right;
    }
    // if we were selected, move this object towards our desired position.
    void Update()
    {
        if (didGetSelected)
        {
            thisRb.MovePosition(Vector2.Lerp(thisRb.position, newPos, 0.25f));
        }        
    }
}
