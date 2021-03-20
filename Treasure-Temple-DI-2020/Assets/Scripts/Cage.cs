using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : Interactable
{
    // The Lion's cage, an interactable

    // we need an id to keep track of which cage is which
    public int id;

    // All interactable objects must implement this method.
    public void TriggerOnInteractMethod(PlayerScript ps)
    {
        OnInteractWith(ps);
    }
    public override void OnInteractWith(PlayerScript ps)
    {
        int i = 0;
        // ah, another one of my silly debug statements.
        Debug.Log("Gaem is not cring");
        foreach (GameObject pKey in ps.inventory)
        {
            // check if we have a cage key in our inventory
            CageKey ck;
            try { ck = pKey.GetComponent<CageKey>(); } catch { ck = null; }
            if (ck != null)
            {
                Debug.Log("Used Cage Key");
                // if this is the second cage, remove the key from our inventory (1 is the second item in a zero-indexed array)
                if (id ==  1)
                {
                    ps.inventory[i] = null;
                    ps.isFull[i] = false;
                }
                // become inactive after we use the key, allowing the character to pass through
                this.gameObject.SetActive(false);
            }
            i++;
        }
        //base.OnInteractWith(ps);
    }
}
