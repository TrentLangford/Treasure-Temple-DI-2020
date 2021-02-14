using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : Interactable
{
    public int id;
    public void TriggerOnInteractMethod(PlayerScript ps)
    {
        OnInteractWith(ps);
    }
    public override void OnInteractWith(PlayerScript ps)
    {
        int i = 0;
        Debug.Log("Gaem is not cring");
        foreach (GameObject pKey in ps.inventory)
        {
            CageKey ck;
            try { ck = pKey.GetComponent<CageKey>(); } catch { ck = null; }
            if (ck != null)
            {
                Debug.Log("Used Cage Key");
                if (id ==  1)
                {
                    ps.inventory[i] = null;
                    ps.isFull[i] = false;
                }
                this.gameObject.SetActive(false);
            }
            i++;
        }
        //base.OnInteractWith(ps);
    }
}
