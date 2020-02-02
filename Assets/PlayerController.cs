using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // What is the maximum speed we want Bob to walk at
    private float maxSpeed = 10f;

    // Start facinf left (like the sprite-sheet)
    private bool facingLeft = false;

    // Check if on ground
    private bool isDashing = false;
    public int numDashes = 2;
    private float dashDuration = 0.3f;
    private float dashCooldown = 0.5f;
    private float nextDash = 1f;

    // This will be a reference to the RigidBody2D 
    // component in the Player object
    private Rigidbody2D rb;

    // This is a reference to the Animator component
    private Animator anim;
    private Gun gun;


    // We initialize our two references in the Start method
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gun = GetComponent<Gun>();
    }

    void resetDash()
    {
        //Debug.Log("End Dash");
        rb.velocity = new Vector2(rb.velocity.x * 0.1f, rb.velocity.y * 0.1f);
        isDashing = false;
    }

    // We use FixedUpdate to do all the animation work
    void FixedUpdate()
    {

        // Get the extent to which the player is currently pressing left or right
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector2 direction= new Vector2(h, v).normalized;
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }

        // Move the RigidBody2D (which holds the player sprite)
        // on the x axis based on the stae of input and the maxSpeed variable
        if (Time.time > nextDash && isDashing == false && Input.GetKeyDown("space") && (numDashes > 0 || gun.hooked))
        {
            gun.meteor = null;
            isDashing = true;
            // Debug.Log("Dash");
            numDashes--;
            nextDash += dashCooldown;
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
            rb.AddRelativeForce(direction * maxSpeed, ForceMode2D.Impulse);

            Invoke("resetDash", dashDuration);
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

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.CompareTag("Ground"))
        {
     
            numDashes = 2; // if the player is in the ground, the bool "canjump" become true
        }

    }
    /*
    void reverseImage()
    {
        // Switch the value of the Boolean
        facingLeft = !facingLeft;

        // Get and store the local scale of the RigidBody2D
        Vector2 theScale = rb.transform.localScale;

        // Flip it around the other way
        theScale.x *= -1;
        rb.transform.localScale = theScale;
    }*/

}
