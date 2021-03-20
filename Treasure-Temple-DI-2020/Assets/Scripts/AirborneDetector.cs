using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneDetector : MonoBehaviour
{
    // A small script that detects if the player is airborne.
    public Transform bottomRight;
    public Transform topLeft;
    public LayerMask whatIsGround;
    private void FixedUpdate()
    {
        // looks at the area defined by the bottom right and top left variables and determines if any colliders
        // in the whatIsGround layer are inside of this area
        this.GetComponentInParent<PlayerScript>().isAirborne = !Physics2D.OverlapArea(topLeft.position, bottomRight.position, whatIsGround);
    }
}
