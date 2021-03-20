using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // unused variables from when the boss would chase you when he found you. Scary!
    public float speed;
    public bool detected;

    public Animation death;
    public Sprite deathSprite;

    // gets the boss's death animation for later use.
    void Awake()
    {
        death = GetComponent<Animation>();
    }
    // plays the boss's death animation when called.
    public void Die()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = deathSprite;
        death.Play();
    }
}
