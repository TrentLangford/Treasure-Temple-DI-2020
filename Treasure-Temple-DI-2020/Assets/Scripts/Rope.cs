using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : Interactable
{
    // An interactable class responsible for all rope behaviors
    public bool isHorizontal;
    public float LeftBound;
    public float RightBound;
    public float BottomBound;
    public float TopBound;
    
    void Awake()
    {
        // set the rope boundaries based on whether the rope is horizontal or not.
        // if the rope is horizontal, then set the bounds to:
        // - left: rope position x minus half of the local scale x
        // - right: rope position x plus half of the local scale x
        if (isHorizontal)
        {
            LeftBound = this.transform.position.x - this.transform.localScale.x / 2;
            RightBound = this.transform.position.x + this.transform.localScale.x / 2;
            BottomBound = TopBound = 0;
        }
        // if the rope is vertical, then set the bounds to:
        // - bottom: rope position y minus half of the local scale y
        // - right: rope position y plus half of the local scale y
        else
        {
            BottomBound = this.transform.position.y - this.transform.localScale.y / 2;
            TopBound = this.transform.position.y + this.transform.localScale.y / 2;
            LeftBound = RightBound = 0;
        }
        // This gives us the bounds of our ropes. If the given bound does not apply, then set the bounds to zero.
    }

    // All interactables must implement this method.
    public void TriggerOnInteractMethod(PlayerScript ps)
    {
        OnInteractWith(ps);
    }
    // We must implement this method to disinteract with this object.
    public void TriggerOnDisinteractMethod(PlayerScript ps)
    {
        OnDisinteractWith(ps);
    }

    public override void OnInteractWith(PlayerScript ps)
    {
        // set the player's rope bounds variables to their respective values, as calculated in the Awake method.
        if (isHorizontal)
        {
            ps.transform.position = new Vector2(Mathf.Clamp(ps.transform.position.x, LeftBound, RightBound), this.transform.position.y);
            ps.ropeLbound = LeftBound;
            ps.ropeRbound = RightBound;
            ps.ropeY = this.transform.position.y;
        }
        if (!isHorizontal)
        {
            ps.transform.position = new Vector2(this.transform.position.x, Mathf.Clamp(ps.transform.position.y, BottomBound, TopBound));
            ps.ropeBbound = BottomBound;
            ps.ropeTbound = TopBound;
            ps.ropeX = this.transform.position.x;
        }
        // set some important variables and disable gravity
        ps.isInteracting = true;
        ps.playerRb.gravityScale = 0;
        ps.playerRb.velocity = new Vector2(ps.playerRb.velocity.x, 0);
        ps.rope = isHorizontal;
        //base.OnInteractWith(ps);
    }
    public override void OnDisinteractWith(PlayerScript ps)
    {
        // reset some important variables and reset the gravity scale.
        ps.isInteracting = false;
        ps.playerRb.gravityScale = 1;
        ps.playerRb.velocity = new Vector2(ps.playerRb.velocity.x, ps.jumpf);
        ps.rope = true;
        ps.ropeLbound = ps.ropeRbound = 0;
        //base.OnDisinteractWith(ps);
    }
}
