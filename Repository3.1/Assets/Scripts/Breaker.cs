using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breaker : MonoBehaviour
{
    [SerializeField]
    protected float weightLimit = 16;
    private Collider[] colliders;
    private Vector3 range;
    private Vector3 offset;
    Rigidbody rb;
    private bool delay;
    private float mass;

    // Use this for initialization
    void Start()
    {
        range = new Vector3(4.5f, 0.5f, 0.5f);
        offset = new Vector3(0, 1, 0);
        delay = false;
        mass = 0;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (delay == false) // We dont need to check for changes every frame, every second will do.
        {
            StartCoroutine(CheckWeightOnTop());
        }
    }

    private IEnumerator CheckWeightOnTop()
    {
        colliders = Physics.OverlapBox(gameObject.transform.position + offset, range); // Get all colliders on top of the pressureplate.
        foreach (Collider c in colliders) // Find their rspective rigid body
        {
            rb = c.gameObject.GetComponent<Rigidbody>() as Rigidbody;
            if (rb != null)
            {
                mass += rb.mass;  // Calculate the total mass on the pressureplate.                     
            }
        }
        if (mass > weightLimit) //Do stuff here.
        {
            Destroy(this.gameObject);
        }
        mass = 0;   //Reset mass.
        delay = true;   //Engae the delay.
        yield return new WaitForSeconds(1f);
        delay = false;  //End the delay.
    }
}
