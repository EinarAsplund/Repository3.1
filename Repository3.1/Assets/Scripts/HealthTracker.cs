﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthTracker : MonoBehaviour {

    [SerializeField]
    private Image heart1, heart2, heart3;
    private int health = 3;

    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Death")
        {
            player.transform.position = respawnPoint.transform.position;
            health -= 1;
            SetHearts(health);
        }
    }

    private void SetHearts(int health)
    {
        switch (health)
        {
            case 3:
                heart1.enabled = true;
                heart2.enabled = true;
                heart3.enabled = true;
                break;
            case 2:
                heart1.enabled = true;
                heart2.enabled = true;
                heart3.enabled = false;
                break;
            case 1:
                heart1.enabled = true;
                heart2.enabled = false;
                heart3.enabled = false;
                break;
            case 0:
                heart1.enabled = false;
                heart2.enabled = false;
                heart3.enabled = false;
                //Initiate End Game
                break;
        }

    }
}
