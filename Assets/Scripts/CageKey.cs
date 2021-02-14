using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageKey : Interactable
{
    public bool didGetSelected;
    public Rigidbody2D thisRb;
    Vector2 newPos;

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

    public void Select(PlayerScript ps)
    {
        didGetSelected = true;
        newPos = new Vector2(ps.transform.position.x, ps.transform.position.y) + Vector2.right;
    }
    void Update()
    {
        if (didGetSelected)
        {
            thisRb.MovePosition(Vector2.Lerp(thisRb.position, newPos, 0.25f));
        }        
    }
}
