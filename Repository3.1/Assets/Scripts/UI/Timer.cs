using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	private float timeLeft = 60;
    [SerializeField]
    private Text timerText;
    [SerializeField]
    private HealthTracker ht;
    // Update is called once per frame
    void Update () {

        if (timeLeft >= 0)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = timeLeft.ToString("F2");
        }
        if (timeLeft <= 0) //Remove this if lava should be deadly.
        {
            ht.Invoke("EndGame", 0.1f);
        }
    }
}
