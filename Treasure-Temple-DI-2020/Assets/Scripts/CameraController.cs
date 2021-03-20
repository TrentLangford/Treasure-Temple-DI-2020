using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Handles looking at 2 players at once. can also unlock and fly freely, a feature that was not used while filming.
    public Transform player1;
    public Transform player2;
    public bool locked = true;
    public Vector3 offset;
    public float snapSpeed = 1f;
    public float dist;

    // set our position to player 1's position
    void Start()
    {
        this.transform.position = new Vector3(player1.position.x, player1.position.y, this.transform.position.z);
    }

    void Update()
    {
        // if we are pressing the unlock/lock key, unlock or lock the camera
        if (Input.GetKeyDown("\\"))
        {
            locked = !locked;
        }

        // if we are not locked, allow our camera to move freely by
        // adjusting the offset from the position right inbetween the 2 characters
        if (!locked)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                offset.x -= 1;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                offset.x += 1;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                offset.y -= 1;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                offset.y += 1;
            }
            // set our viewsize to the default
            this.GetComponent<Camera>().orthographicSize = 10;
        }
        // if we are locked:
        else
        {
            // linearlly interpolate our offset to zero, which will smoothy snap the camera back to the
            // center of the 2 characters if we just locked
            offset = Vector3.Lerp(offset, Vector3.zero, snapSpeed);
            // set the camera's viewsize to an estimation I came up with that keeps both characters in the viewport very well
            this.GetComponent<Camera>().orthographicSize = Mathf.Clamp((dist / 22) * 11.5f, 5, float.MaxValue);
        }

        // the distance between the 2 characters
        dist = Vector2.Distance(player1.position, player2.position);
        
        // set the new pos inbetween the 2 characters and set the camera's positon to this position
        Vector2 newPos = Vector2.Lerp((player1.position + player2.position)/2, (player1.position + player2.position)/2 + offset, snapSpeed);
        this.transform.position = new Vector3(newPos.x, newPos.y, this.transform.position.z);
    }
}
