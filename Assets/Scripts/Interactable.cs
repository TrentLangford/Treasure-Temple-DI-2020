using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    /*
        The base class for an interactable. All interactables can override these methods, which set some unused ui stuff.
        would've been much better if I could've implemented polymorphism with pointers but
        that was too much for this short period of time. Something I would have worked on if I was given
        another month.
    */
    public virtual void OnInteractWith(PlayerScript ps)
    {
        UIManager.instance.EnableInteractText(this.gameObject.name, "to interact with", ps.interact, false);
    }
    public virtual void OnDisinteractWith(PlayerScript ps)
    {
        UIManager.instance.EnableInteractText(this.gameObject.name, "to stop interacting with", ps.interact, false);
    }
}
