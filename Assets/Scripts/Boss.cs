using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float speed;
    public bool detected;
    public Animation death;
    public Sprite deathSprite;

    void Awake()
    {
        death = GetComponent<Animation>();
    }
    public void Die()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = deathSprite;
        death.Play();
    }
}
