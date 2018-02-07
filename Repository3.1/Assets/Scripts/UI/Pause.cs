using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

    public void EnablePause()
    {
        Time.timeScale = 0;
        //Enable pause menue
    }
    public void EnablePlay()
    {
        Time.timeScale = 1;
        //Disable
    }
}
