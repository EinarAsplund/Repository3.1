using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAppear : MonoBehaviour {

    [SerializeField] private Text customText;
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPoint;

    private static float lastTeleport;

    private bool doorOpen = true;
    public bool DoorOpen
    {
        set { doorOpen = value; }
    }

    private bool CanTeleport
    {
        get { return Time.time > lastTeleport + 0.5f ? true : false; }
    }

    private bool PlayerIsNearby
    {
        get; set;
    }

    void Awake()
    {
    }

    void Update()
    {
        if (PlayerIsNearby)
        {
            if (doorOpen && Input.GetKeyDown(KeyCode.E) && CanTeleport)
            {
                lastTeleport = Time.time;
                player.transform.position = respawnPoint.transform.position;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (doorOpen && other.CompareTag("Player"))
        {
            PlayerIsNearby = true;
            customText.enabled = true;
        }

    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerIsNearby = false;
            customText.enabled = false;
        }
    }   
}
