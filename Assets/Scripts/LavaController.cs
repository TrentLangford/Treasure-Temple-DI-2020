using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour
{
    // A small class responsible for controlling the individual lava components.
    public LavaComponent[] components;
    public float timeBtwSwitch;
    private float countdownTimer;

    // resets the countdown timer and the time btw switch, also turns on every other lava component
    private void Start()
    {
        countdownTimer = timeBtwSwitch;
        int i = 0;
        foreach (LavaComponent component in components)
        {
            if (i % 2 == 0) component.isActive = true;
            i++;
        }
    }
    private void Update()
    {
        // decrement the countdown timer
        countdownTimer -= Time.deltaTime;
        // if a certian amount of time has passed, switch which components are on and off and resets the timer
        if (countdownTimer <= 0)
        {
            foreach (LavaComponent component in components)
            {
                component.isActive = !component.isActive;
            }
            countdownTimer = timeBtwSwitch;
        }
    }
}
