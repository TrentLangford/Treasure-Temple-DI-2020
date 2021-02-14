using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    private void FixedUpdate()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, new Vector2(this.transform.position.x + speed, this.transform.position.y), speed);
    }
}
