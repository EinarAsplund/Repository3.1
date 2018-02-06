using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlat : MonoBehaviour {

    public GameObject MovingPlatform;
    public GameObject ThePlayer;

     void OnTriggerEnter()
    {
        ThePlayer.transform.parent = MovingPlatform.transform;
    }

}
