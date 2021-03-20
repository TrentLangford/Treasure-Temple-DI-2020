using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // A small class responible for all of the moving platforms in the game
    public Animator anim;
    public bool active;
    public bool didActive;
    public string activate;
    public string deactivate;

    // resets some variables
    private void Awake()
    {
        active = false;
        didActive = false;
    }
    private void FixedUpdate()
    {
        // if the platform is active and has not triggered its active animation yet, play the animation
        if (active && !didActive)
        {
            anim.SetTrigger(activate);
            didActive = true;
        }
        // if the platform is not active and it already triggered its active animation yet, play the deactive animation
        if (!active && didActive)
        {
            anim.SetTrigger(deactivate);
            didActive = false;
        }
    }
}
