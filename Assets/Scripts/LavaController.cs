using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour
{
    public LavaComponent[] components;
    public float timeBtwSwitch;
    private float countdownTimer;

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
        countdownTimer -= Time.deltaTime;
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
