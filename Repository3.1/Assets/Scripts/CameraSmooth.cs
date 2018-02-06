using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmooth : MonoBehaviour {

    public Transform target;


    public float smoothSpeed = 0.1f;
    public Vector3 offset;


    void LateUpdate ()

    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.MoveTowards(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);

    }


}
