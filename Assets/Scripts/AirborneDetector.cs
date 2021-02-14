using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneDetector : MonoBehaviour
{
    public Transform bottomRight;
    public Transform topLeft;
    public LayerMask whatIsGround;
    private void FixedUpdate()
    {
        this.GetComponentInParent<PlayerScript>().isAirborne = !Physics2D.OverlapArea(topLeft.position, bottomRight.position, whatIsGround);
    }
}
