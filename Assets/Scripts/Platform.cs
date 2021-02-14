using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Animator anim;
    public bool active;
    public bool didActive;
    public string activate;
    public string deactivate;
    private void Awake()
    {
        active = false;
        didActive = false;
    }
    private void FixedUpdate()
    {
        if (active && !didActive)
        {
            anim.SetTrigger(activate);
            didActive = true;
        }
        if (!active && didActive)
        {
            anim.SetTrigger(deactivate);
            didActive = false;
        }
    }
}
