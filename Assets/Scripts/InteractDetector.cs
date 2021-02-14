﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Interactables"))
        {
            this.GetComponentInParent<PlayerScript>().InteractableFind(collision.GetComponent<Interactable>(), true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Interactables"))
        {
            this.GetComponentInParent<PlayerScript>().InteractableFind(collision.GetComponent<Interactable>(), false);
        }
    }
}
