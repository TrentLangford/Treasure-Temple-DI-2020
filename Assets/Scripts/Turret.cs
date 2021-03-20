using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    // An unused class that spawned projectiles in at a regular rate and destroyed them after 5 seconds.
    public GameObject projectile;
    public float fireRate;
    private float timeBtwShots;

    private void Update()
    {
        if (timeBtwShots < 0)
        {
            GameObject newBullet = Instantiate(projectile, new Vector3(this.transform.position.x + 1, this.transform.position.y), Quaternion.identity);
            Destroy(newBullet, 5f);
            timeBtwShots = fireRate;
        }
        timeBtwShots -= Time.deltaTime;

    }
}
