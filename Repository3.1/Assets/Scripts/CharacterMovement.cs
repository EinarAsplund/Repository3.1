using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    private Animator animator;
    private CharacterController cc;
    private float speed = 5f;
    private float jumpSpeed = 20f;
    private float gravity = 50;
    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody rb;
    private Vector3 wallClimb = new Vector3(4, 20, 0);
    private Vector3 wallDrop = new Vector3(1, 20, 0);
    private Vector3 wallLeap = new Vector3(10, 10, 0);
    private float pushF = 0f; //Change to make boxes movable or not.
    private ControllerColliderHit hit;
    private ControllerColliderHit previousHit;
    private bool isWallSliding = false;
    private float slideSpeed = 1;
    private int maxWallJumps = 1;
    private int currentWallJumps = 0;
    private float slideTimer = 0.5f;

    Transform thingToPush; // null if nothing, else a link to some pullable crate.

    Quaternion lookRight;
    Quaternion lookLeft;

    // Use this for initialization
    private void Start ()
    {
        cc = gameObject.GetComponent<CharacterController>() as CharacterController;
        rb = gameObject.GetComponent<Rigidbody>() as Rigidbody;
        animator = GetComponent<Animator>();
        // assuming character starts looking to the right:
        lookRight = transform.rotation;
        // calculate rotation flipped 180 degrees:
        lookLeft = lookRight * Quaternion.Euler(0, 180, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        MoveCrate();
        GroundMovement();

        //Fungerar ej. Låt stå tillfälligt.
        //else if (!ch.isGrounded)
        //{
        //    moveDirection.x = Input.GetAxis("Horizontal") * speed;
        //} 


      

        moveDirection.y -= gravity * Time.deltaTime;    
        cc.Move(moveDirection * Time.deltaTime);    //Apply gravity & perform move.
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        previousHit = this.hit;
        this.hit = hit;

        if (hit.gameObject.tag == "Crate" && cc.isGrounded )  //Find a pushable object.
        {
            Push();
        }


        if (!cc.isGrounded && hit.gameObject.tag == "Wall") //When hitting a wall and not on the ground, do stuff.
        {
            WallSliding();
            WallJumping();                     
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            other.gameObject.GetComponent<AudioSource>().Play();
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            Destroy(other.gameObject, 0.3f);
        }
    }

    private void GroundMovement() // Move on he ground.
    {
        if (cc.isGrounded)
        {
            animator.SetBool("Jump", false);
            previousHit = null;
            hit = null;
            currentWallJumps = 0;

            //Uncomment & turn character 90 degrees to activate switching directions
            moveDirection = Vector3.zero;
            if (Input.GetKey(KeyCode.D))
            {
                transform.rotation = lookRight;
                moveDirection = Vector3.forward;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.rotation = lookLeft;
                moveDirection = Vector3.forward;
            }

            //moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 0); //Get the value of the virtual x axis. Y and Z are set to zero.
            moveDirection = transform.TransformDirection(moveDirection);    //Transform direction from local to worldspace.
            moveDirection *= speed; //Multiply the direction of travel with the speed.
            animator.SetFloat("BlendX", Input.GetAxis("Horizontal"));

          if (animator.GetFloat("BlendX") > 0 || animator.GetFloat("BlendX") < 0)
            {
                animator.SetBool("Running", true);
            }
          else
            {
                animator.SetBool("Running", false);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                moveDirection.y = jumpSpeed;    //Jump if space is pressed.
                animator.SetBool("Running", false);
                animator.SetBool("Jump", true);
               

            }
        }
    }

    private void Push()
    {
       
        StartCoroutine(Pushing());
    }

    IEnumerator Pushing()
    {
        animator.SetBool("Pushing", true);
        thingToPush = hit.transform;
        float timer = 10;
        while (timer > 0 && animator.GetBool("Running") == false && animator.GetBool("Jump") == false)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        animator.SetBool("Pushing", false);
    }

    private void WallSliding() // Slide on walls for some duration.
    {
        if (!isWallSliding && previousHit == null || !isWallSliding && !ReferenceEquals(hit.gameObject, previousHit.gameObject))
        {
            StartCoroutine("IsWallSliding");
        }

        else if (isWallSliding)
        {
            moveDirection.y = -slideSpeed;   //Aoutomaticly slide slowly along walls.
        }
    }

    private void WallJumping() // Jump from a wall. This function could use some cleaning.
    {
        if (isWallSliding && currentWallJumps < maxWallJumps)
        {
            if (moveDirection.x > 0 && Input.GetKey(KeyCode.Space))    //If walls are to the right, do stuff.
            {
                if (Input.GetKey(KeyCode.D))
                {
                    moveDirection.x = hit.normal.x * wallClimb.x;
                    moveDirection.y = wallClimb.y;
                    currentWallJumps += 1;
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    moveDirection.x = hit.normal.x * wallLeap.x;
                    moveDirection.y = wallLeap.y;
                    currentWallJumps += 1;
                }           
            }

            else if (moveDirection.x < 0 && Input.GetKey(KeyCode.Space))    //If walls are to the left, do other stuff.
            {
                if (Input.GetKey(KeyCode.A))
                {
                    moveDirection.x = hit.normal.x * wallClimb.x;
                    moveDirection.y = wallClimb.y;
                    currentWallJumps += 1;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    moveDirection.x = hit.normal.x * wallLeap.x;
                    moveDirection.y = wallLeap.y;
                    currentWallJumps += 1;
                }
            }
            else if (Input.GetKeyUp(KeyCode.Space)) // if the player releases space, drop of the ledge.
            {
                moveDirection.x = hit.normal.x * wallDrop.x;
                moveDirection.y = wallDrop.y;
                currentWallJumps += 1;
            }            
        }       
    }

    private void MoveCrate() // Push objects.
    {
        if (thingToPush != null)
        {
            Vector3 D = gameObject.transform.position - thingToPush.position; // line from crate to player
            float dist = D.magnitude;
            Vector3 pullDir = -D.normalized; // short line from player to crate. Change to positive for pulling
            if (dist > 1) thingToPush = null; // lose tracking if too far
            else if (dist >= 0.5)
            { // push if withing the 0-1 range.
                thingToPush.GetComponent<Rigidbody>().velocity += pullDir * (pushF * Time.deltaTime);                
            }
        }
    }
    
    private IEnumerator IsWallSliding() // Activate wallsliding for some time.
    {
        isWallSliding = true;
        yield return new WaitForSeconds(slideTimer);
        isWallSliding = false;
    }

    // Properties
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public float JumpSpeed
    {
        get { return jumpSpeed; }
        set { jumpSpeed = value; }
    }

    public float SlideSpeed
    {
        get { return slideSpeed; }
        set { slideSpeed = value; }
    }

    public float SlideTimer
    {
        get { return slideTimer; }
        set { slideTimer = value; }
    }

    public float Gravity
    {
        get { return gravity; }
        set { gravity = value; }
    }

    public float PushF
    {
        get { return pushF; }
        set { pushF = value; }
    }

    public int MaxWallJumps
    {
        get { return maxWallJumps; }
        set { maxWallJumps = value; }
    }

    public int CurrentWallJumps
    {
        get { return currentWallJumps; }
        set { currentWallJumps = value; }
    }

    public Rigidbody RB
    {
        get { return rb; }
    }

    public Vector3 WallDrop
    {
        get { return wallDrop; }
        set { wallDrop = value; }
    }

    public Vector3 WallClimb
    {
        get { return wallClimb; }
        set { wallClimb = value; }
    }

    public Vector3 WallLeap
    {
        get { return wallLeap; }
        set { wallLeap = value; }
    }
}
