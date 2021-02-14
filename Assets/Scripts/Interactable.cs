using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void OnInteractWith(PlayerScript ps)
    {
        UIManager.instance.EnableInteractText(this.gameObject.name, "to interact with", ps.interact, false);
    }
    public virtual void OnDisinteractWith(PlayerScript ps)
    {
        UIManager.instance.EnableInteractText(this.gameObject.name, "to stop interacting with", ps.interact, false);
    }
}
