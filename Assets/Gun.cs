using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Gun : MonoBehaviour
{
    public bool ready = true;
    /// <summary>
    // setup layers to select which entities to hit
    /// </summary>
    public LayerMask whatToHit;

    public UnityEvent goOut;

    public float hookedVelocity;
    bool hooked = false;
    Transform shootPoint;
    GameObject hookLine;

    Rigidbody2D rb;
    private PlayerController controller;
    // null if not standing on meteor set to null after jumping
    public Transform meteor = null;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        shootPoint = transform.Find("FiringOrigin");
        if (shootPoint == null)
        {
            Debug.LogError("No Firing Origin.");
        }
        hookLine = GameObject.Find("HookLine");
        if (shootPoint == null)
        {
            Debug.LogError("No look line.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ready)
        {
            // Fire1 should be setup in the input manager else use the following line
            // Edit->Project Settings->Input
            //if (Input.GetButtonDown("Fire1"))
            if (Input.GetKeyDown(KeyCode.E))
            {
                ready = false;
                Shoot();
            }
            else if (meteor != null)
            {
                // meteor != null implies standing
                transform.position = new Vector3(meteor.transform.position.x, meteor.transform.position.y, transform.position.z);
                
            }
        }
        else if (hooked == true)
        {
            // cancel hook
            if (Input.GetKeyDown(KeyCode.Space))
            {
                hookLine.GetComponent<MoveTrail>().takeBack();
                hooked = false;
            }
            else
            {
                // get pulled
                var vec = hookLine.transform.position - transform.position;
                // remove z component
                vec.z = 0;
                vec = vec.normalized * hookedVelocity;
                rb.velocity = new Vector2(vec.x, vec.y);
            }
        }
    }

    // shoot the grapple *I hope*
    void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 shootPointPosition = new Vector2(shootPoint.position.x, shootPoint.position.y);
        // RaycastHit2D hit = Physics2D.Raycast(shootPointPosition, mousePosition - shootPointPosition, maxDistance, whatToHit);
        Effect(Quaternion.Euler(0,0,Mathf.Rad2Deg*(Mathf.Atan2((mousePosition - shootPointPosition).y, (mousePosition - shootPointPosition).x))));
        Debug.DrawLine(shootPointPosition, 100*(mousePosition - shootPointPosition), Color.red);
        /*if (hit.collider != null)
        {
            Debug.DrawLine(shootPointPosition, hit.point, Color.cyan);
        }*/
    }

    void Effect(Quaternion dir)
    {
        hookLine.transform.position = shootPoint.position;
        hookLine.transform.rotation = dir;
        // Instantiate(BulletTrailPrefab, shootPoint.position, dir);
        goOut.Invoke();
    }

    public void hookedOn()
    {
        hooked = true;
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
        hooked = false;
        rb.velocity = Vector2.zero;
        controller.numDashes = 2;
    }
}
