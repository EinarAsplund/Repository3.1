using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthTracker : MonoBehaviour {

    [SerializeField]
    private Image heart1, defeat, victory;
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Button pauseButton, playButton;
    private int health = 3;
    private bool victoryCheck = false;

    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPoint;

    private bool invincible = false;
    private ShrinkAndGrow sag;

    public Transform RespawnPoint
    {
        set { respawnPoint = value; }
    }

    public bool VictoryCHeck
    {
        set { victoryCheck = value; }
    }

    private void Start()
    {
        sag = gameObject.GetComponent<ShrinkAndGrow>() as ShrinkAndGrow;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Death")
        {
            player.transform.position = respawnPoint.transform.position;
            if (!invincible)
            {
                health--;
                if(health <= 0)
                {
                    health = 0;
                    Invoke("EndGame", 0.1f);
                }
            }

            SetHearts(health);
            StartCoroutine("Delay");
        }
    }

    private IEnumerator Delay()
    {
        invincible = true;
        yield return new WaitForSeconds(1f);
        invincible = false;
    }

    private void EndGame()
    {
        if (health <= 0)
        {
            defeat.enabled = true;
        }
        else if (victoryCheck)
        {
            victory.enabled = true;
        }
        Time.timeScale = 0;
        pauseButton.interactable = false;
        playButton.interactable = false;
        sag.enabled = false;
    }

    private void SetHearts(int health)
    {
        healthText.text = health.ToString();
    }
}
