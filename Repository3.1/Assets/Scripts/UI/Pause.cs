using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

    bool isPaused = false;

    public void EnablePause()
    {
        isPaused = true;
        Time.timeScale = 0;
        //Enable pause menue
    }
    public void EnablePlay()
    {
        isPaused = false;
        Time.timeScale = 1;
        //Disable
    }
}
