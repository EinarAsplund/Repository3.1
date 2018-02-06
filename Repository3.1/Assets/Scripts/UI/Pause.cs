using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

    public void EnablePause()
    {
        Time.timeScale = 0;
    }
}
