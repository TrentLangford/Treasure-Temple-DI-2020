using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public bool locked = true;
    public Vector3 offset;
    public float snapSpeed = 1f;
    public float dist;
    void Start()
    {
        this.transform.position = new Vector3(player1.position.x, player1.position.y, this.transform.position.z);
    }

    void Update()
    {
        if (Input.GetKeyDown("\\"))
        {
            locked = !locked;
        }


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
            this.GetComponent<Camera>().orthographicSize = 10;
        }
        else
        {
            offset = Vector3.Lerp(offset, Vector3.zero, snapSpeed);
            this.GetComponent<Camera>().orthographicSize = Mathf.Clamp((dist / 22) * 11.5f, 5, float.MaxValue);
        }


        dist = Vector2.Distance(player1.position, player2.position);
        

        Vector2 newPos = Vector2.Lerp((player1.position + player2.position)/2, (player1.position + player2.position)/2 + offset, snapSpeed);
        this.transform.position = new Vector3(newPos.x, newPos.y, this.transform.position.z);
    }
}
