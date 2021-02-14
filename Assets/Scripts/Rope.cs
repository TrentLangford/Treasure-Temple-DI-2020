using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : Interactable
{
    public bool isHorizontal;
    public float LeftBound;
    public float RightBound;
    public float BottomBound;
    public float TopBound;
    
    void Awake()
    {
        if (isHorizontal)
        {
            LeftBound = this.transform.position.x - this.transform.localScale.x / 2;
            RightBound = this.transform.position.x + this.transform.localScale.x / 2;
            BottomBound = TopBound = 0;
        }
        else 
        {
            BottomBound = this.transform.position.y - this.transform.localScale.y / 2;
            TopBound = this.transform.position.y + this.transform.localScale.y / 2;
            LeftBound = RightBound = 0;
        }
        
    }
    public void TriggerOnInteractMethod(PlayerScript ps)
    {
        OnInteractWith(ps);
    }
    public void TriggerOnDisinteractMethod(PlayerScript ps)
    {
        OnDisinteractWith(ps);
    }

    public override void OnInteractWith(PlayerScript ps)
    {
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
        ps.isInteracting = true;
        ps.playerRb.gravityScale = 0;
        ps.playerRb.velocity = new Vector2(ps.playerRb.velocity.x, 0);
        ps.rope = isHorizontal;
        //base.OnInteractWith(ps);
    }
    public override void OnDisinteractWith(PlayerScript ps)
    {
        ps.isInteracting = false;
        ps.playerRb.gravityScale = 1;
        ps.playerRb.velocity = new Vector2(ps.playerRb.velocity.x, ps.jumpf);
        ps.rope = true;
        ps.ropeLbound = ps.ropeRbound = 0;
        //base.OnDisinteractWith(ps);
    }
}
