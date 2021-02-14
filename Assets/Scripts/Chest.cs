using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    public int id;
    public GameObject newItem;
    public void TriggerOnInteractMethod(PlayerScript ps)
    {
        OnInteractWith(ps);
    }
    public override void OnInteractWith(PlayerScript ps)
    {
        int i = 0;
        foreach (GameObject pKey in ps.inventory)
        {
            if (pKey.GetComponent<Key>() != null && pKey.GetComponent<Key>().id == this.id)
            {
                Debug.Log($"Used key {pKey.GetComponent<Key>().id} on chest {id} sucssesfully");
                ps.inventory[i] = null;
                ps.isFull[i] = false;
                Instantiate(newItem, this.transform.position, Quaternion.identity);
                this.gameObject.SetActive(false);
                break;
            }
            i++;
        }
        //base.OnInteractWith(ps);
    }
}
