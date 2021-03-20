using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // A cut class for a projectile that would respawn the character when they hit it. the class just moves the projectile to the right by a steady speed
    public float speed;
    private void FixedUpdate()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, new Vector2(this.transform.position.x + speed, this.transform.position.y), speed);
    }
}
