using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{

    // What is the maximum speed we want Bob to walk at
    private float maxSpeed = 10f;
    // ready to shoot hook
    bool ready = true;
    public float bonusSPEED;
    public UnityEvent goOut;

    public float hookedVelocity;
    //public bool hooked = false;

    // Check if on ground
    public Dash currentDash = null;
    private Dash nextDash = null;
    public int numDashes = 2;
    public float dashDuration = 0.3f;
    public float dashCooldown = 0.5f;
    public float dashTimer;

    // This will be a reference to the RigidBody2D 
    // component in the Player object
    private Rigidbody2D rb;

    // This is a reference to the Animator component
    private Animator anim;
    private Gun gun;
    Transform shootPoint;
    GameObject hookLine;

    // dash frame window
    public static int frameWindow = 4;

    public int ignoreCounter = 0;
    public Transform meteor = null;
    bool dashed;

    public playerStates state = playerStates.inAir;

    // We initialize our two references in the Start method
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gun = GetComponent<Gun>();
        shootPoint = transform.Find("FiringOrigin");
        if (shootPoint == null)
        {
            Debug.LogError("No Firing Origin.");
        }
        hookLine = GameObject.Find("HookLine");
        if (shootPoint == null)
        {
            Debug.LogError("No hook line.");
        }
    }

    void ResetDash()
    {
        //Debug.Log("End Dash");
        if (ignoreCounter == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * 0.1f, rb.velocity.y * 0.1f);
            currentDash = null;
        }
        else
        {
            ignoreCounter--;
        }
    }

    // We use FixedUpdate to do all the animation work
    void Update()
    {
        dashed = false;
        dashTimer -= Time.deltaTime;
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }

        // Get the extent to which the player is currently pressing left or right
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector2 direction= new Vector2(h, v);
        // update queued dash timer
        if (nextDash != null && nextDash.CheckCounter())
        {
                nextDash.UpdateDir(direction);
        }

        if (currentDash == null && (numDashes > 0 || state == playerStates.hooked) && dashTimer <= 0 && nextDash != null)
        {
            // we move the dash from queue to current
            DoDash(nextDash);
            nextDash = null;
        }
        else if (Input.GetKeyDown("space") && (numDashes > 0 || state == playerStates.hooked))
        {
            // can dash create current dash
            if (currentDash == null && dashTimer <= 0 )
            {
                DoDash(new Dash(direction));
            }
            else
            {
                // add a dash to the queue
                if (nextDash == null)
                    nextDash = new Dash(direction);
            }
        }
        else if (currentDash != null && currentDash.CheckCounter())
        {
            // we can still adjust the dash
            currentDash.UpdateDir(direction);
            float vnorm = rb.velocity.magnitude;
            rb.velocity = new Vector2 (vnorm * currentDash.direction.normalized.x, vnorm * currentDash.direction.normalized.y);
        }

        // Pass in the current velocity of the RigidBody2D
        // The speed parameter of the Animator now knows
        // how fast the player is moving and responds accordingly
        //Debug.Log(rb.velocity.x);
        anim.SetFloat("Velocity", rb.velocity.x);

        // Check which way the player is facing 
        // and call reverseImage if neccessary
        /*if (h < 0 && !facingLeft)
            reverseImage();
        else if (h > 0 && facingLeft)
            reverseImage();*/
        if (ready)
        {
            // Fire1 should be setup in the input manager else use the following line
            // Edit->Project Settings->Input
            //if (Input.GetKeyDown(KeyCode.E))
            if (Input.GetButtonDown("Fire1"))
            {
                ready = false;
                Shoot();
            }
        }
        if (!dashed)
        {
            if (state == playerStates.hooked)
            {
                // get pulled
                var vec = hookLine.transform.position - transform.position;
                // remove z component
                vec.z = 0;
                vec = vec.normalized * hookedVelocity;
                rb.velocity = new Vector2(vec.x, vec.y);
            }
            else if (meteor != null)
            {
                // meteor != null implies standing
                transform.position = new Vector3(meteor.transform.position.x, meteor.transform.position.y, transform.position.z);
            }
        }
    }

    void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 shootPointPosition = new Vector2(shootPoint.position.x, shootPoint.position.y);
        Effect(Quaternion.Euler(0, 0, Mathf.Rad2Deg * (Mathf.Atan2((mousePosition - shootPointPosition).y, (mousePosition - shootPointPosition).x))));
    }

    void Effect(Quaternion dir)
    {
        hookLine.transform.position = shootPoint.position;
        hookLine.transform.rotation = dir;
        // Instantiate(BulletTrailPrefab, shootPoint.position, dir);
        goOut.Invoke();
    }

    /// <summary>
    /// Does the dash taking a dash object
    /// </summary>
    /// <param name="direction"></param>
    void DoDash(Dash dash)
    {
        dashed = true;
        meteor = null;
        currentDash = dash;
        // Debug.Log("Dash");
        numDashes--;
        dashTimer = dashCooldown;

        rb.velocity = new Vector2(0, 0);
        //if ((rb.velocity.x<0 && direction.x >0) || (rb.velocity.x > 0 && direction.x < 0))
        //{
        //    rb.velocity = new Vector2(0, rb.velocity.y);
        //}
        //if ((rb.velocity.y < 0 && direction.y > 0) || (rb.velocity.y > 0 && direction.y < 0))
        //{
        //    rb.velocity = new Vector2(rb.velocity.x,0);
        //}
        rb.angularVelocity = 0;
        if (state != playerStates.hooked)
        {
            rb.AddRelativeForce(currentDash.direction.normalized * maxSpeed, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddRelativeForce(currentDash.direction.normalized * maxSpeed, ForceMode2D.Impulse);
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * bonusSPEED);
            hookLine.GetComponent<MoveTrail>().takeBack();
        }
        state = playerStates.dashing;
        Invoke("ResetDash", dashDuration);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.CompareTag("Ground"))
        {
     
            numDashes = 2; // if the player is in the ground, the bool "canjump" become true
        }

    }

    public enum playerStates
    {
        inAir,
        dashing,
        hooked,
        onMeteor
    }
    public void hookedOn()
    {
        Debug.Log("pulling");
        state = playerStates.hooked;
    }

    public void isReady()
    {
        ready = true;
    }

    public void jumpOn(Transform meteor)
    {
        this.meteor = meteor;
        // remove the hook
        hookLine.GetComponent<MoveTrail>().takeBack();
        // teleport to the rock?
        state = playerStates.onMeteor;
        rb.velocity = Vector2.zero;
        numDashes = 2;
        if (currentDash != null)
        {
            ignoreCounter++;
            currentDash = null;
            dashTimer = 0;
        }
    }

    public class Dash
    {
        public Vector2 direction;
        int counter;
        public Dash(Vector2 direction)
        {
            // we can no longer have no direction
            if (direction.magnitude == 0)
            {
                // default upwards
                direction = new Vector2(0, 1);
            }
            this.direction = direction;
            counter = frameWindow;
        }

        /// <summary>
        /// Returns if we can still adjust
        /// </summary>
        /// <returns></returns>
        public bool CheckCounter()
        {
            if (counter > 0)
            {
                counter--;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// update direction with potential additionnal presses
        /// </summary>
        /// <param name="dir"></param>
        public void UpdateDir (Vector2 dir)
        {
            if (direction.x == 0 && dir.x != 0)
            {
                direction += new Vector2(dir.x, 0);
            }
            if (direction.y == 0 && dir.y != 0)
            {
                direction += new Vector2(0, dir.y);
            }
        }
    }
}
