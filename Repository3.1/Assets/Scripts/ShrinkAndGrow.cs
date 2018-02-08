using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkAndGrow : MonoBehaviour
{
    private float Size;
    private enum CurrentSize {Small = 0, Medium, Large}
    private CurrentSize currentSize;
    private CharacterMovement cm;
    private CharacterController cc;

    private void Start()
    {
        cm = gameObject.GetComponent<CharacterMovement>() as CharacterMovement;
        cc = gameObject.GetComponent<CharacterController>() as CharacterController;
        Size = gameObject.transform.localScale.x;
        currentSize = CurrentSize.Medium;
    }

    private void Update()
    {
        bool temp = ChangeSize();
        if (temp)
        {
            switch (currentSize)
            {
            case CurrentSize.Small:
                    //Change stuff here
                    cm.MaxWallJumps = 2;
                    cm.RB.mass = 5;
                    cm.JumpSpeed = 20;
                    cm.PushF = 0;
                    cm.WallClimb = new Vector3(4, 25, 0);
                    cm.WallLeap = new Vector3(13, 10, 0);
                    cm.SlideTimer = 1;
                    break;
            case CurrentSize.Medium:
                    //Change stuff here
                    cm.MaxWallJumps = 1;
                    cm.RB.mass = 10;
                    cm.JumpSpeed = 25;
                    cm.PushF = 0;
                    cm.WallClimb = new Vector3(4, 20, 0);
                    cm.WallLeap = new Vector3(10, 10, 0);
                    cm.SlideTimer = 0.5f;
                    break;
            case CurrentSize.Large:
                    //Change stuff here
                    cm.MaxWallJumps = 0;
                    cm.RB.mass = 15;
                    cm.JumpSpeed = 30;
                    cm.PushF = 25;
                    cm.SlideTimer = 0;
                    break;
            }
        }
        
    }

    private bool ChangeSize() //Changes the character size if within bondraries.
    {
        if (currentSize != CurrentSize.Large && Input.GetMouseButtonDown(0))
        {
            transform.localScale = new Vector3(transform.localScale.x * 2, transform.localScale.y * 2, transform.localScale.z * 2);
            cc.radius -= 0.2f;
            currentSize += 1;
            return true;
        }
        if (currentSize != CurrentSize.Small && Input.GetMouseButtonDown(1))
        {
            transform.localScale = new Vector3(transform.localScale.x / 2, transform.localScale.y / 2, transform.localScale.z / 2);
            cc.radius += 0.2f;
            currentSize -= 1;
            return true;
        }
        else
            return false;
    }

}
