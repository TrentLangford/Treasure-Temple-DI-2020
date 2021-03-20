using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Console : MonoBehaviour
{
    // Congrats on finding my secret class!
    // See if you can crack my "security!"
    // or don't, I wouldn't be too dissapointed
    // hint: ascii

    public GameObject secret;
    public TMP_InputField console;
    public GameObject solvedtext;

    int[] chars =
    {
        0x4f,
        0x4a,
        0x69,
        0x70,
        0x63,
        0x42,
        0x63,
        0x75,
        0x72,
        0x6f,
        0x68,
        0x67,
        0x72,
        0x6f,
        0x69,
        0x68,
        0x4f,
        0x6b,
        0x67,
        0x61,
        0x6f,
        0x68,
        0x67,
        0x72,
        0x6f,
        0x69,
        0x68
    };
    int key = 0x06;
    char[] pass = new char[27];
    string globalPassStr;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        int i = 0;
        foreach (int chr in chars)
        {
            pass[i] = System.Convert.ToChar(chr ^ key);
            i++;
        }
        string passStr = new string(pass);
        globalPassStr = passStr;
        Debug.Log(globalPassStr);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMultiply))
        {
            PlayerScript[] players = FindObjectsOfType<PlayerScript>();
            foreach (PlayerScript player in players)
            {
                player.active = !player.active;
            }
            secret.SetActive(!secret.activeSelf);
        }
    }

    public void Submit()
    {
        if (console.text == globalPassStr) solvedtext.SetActive(true);
    }

}
