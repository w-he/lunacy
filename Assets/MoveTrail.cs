using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class MoveTrail : MonoBehaviour
{
    bool outThere = false;
    public hookState state = hookState.noHook;
    // public Quaternion direction;
    public float moveSpeed;
    public float distance;
    GameObject origin;
    // <summary>
    // distance such that we stop
    // <summary/>
    public float epsilon;
    float timer;

    Transform meteor;

    public UnityEvent done;
    public UnityEvent hookPull;

    SpriteRenderer spriteRenderer;
    LineRenderer lr;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lr = GetComponent<LineRenderer>();
        origin = GameObject.Find("FiringOrigin");
    }
    float time()
    {
        return distance / moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = origin.transform.position;
        if (state == hookState.hookingOut || state == hookState.hookingIn)
        {
            if (state == hookState.hookingOut)
            {
                timer -= Time.deltaTime;
            }
            if (state == hookState.hookingIn)
            {
                // target origin
                transform.rotation =
                    Quaternion.Euler(0, 0, Mathf.Rad2Deg * (Mathf.Atan2((pos - transform.position).y, (pos - transform.position).x)));
                if ((pos - transform.position).magnitude < epsilon)
                {
                    takeBack();
                    return;
                }
            }
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            lr.SetPosition(0, pos);
            lr.SetPosition(1, transform.position);
            if (timer < 0)
            {
                state = hookState.hookingIn;
            }
        }
        else if (state == hookState.hooked)
        {
            transform.position = meteor.position;
            lr.SetPosition(0, pos);
            lr.SetPosition(1, transform.position);
        }
    }
    // <summary>
    // render sprite and move out
    // <summary/>
    public void MoveOut()
    {
        // transform.position = pos;
        // transform.rotation = dir
        outThere = true;
        state = hookState.hookingOut;
        spriteRenderer.enabled = true;
        timer = time();
        lr.enabled = true;
    }

    public void takeBack()
    {
        outThere = false;
        state = hookState.noHook;
        spriteRenderer.enabled = false;
        lr.enabled = false;
        done.Invoke();
    }

    // landed on a meteor
    public void latchOn(Transform meteor)
    {
        this.meteor = meteor;
        state = hookState.hooked;
        hookPull.Invoke();
    }

    public enum hookState
    {
        noHook,
        hookingOut,
        hookingIn,
        hooked,
    }
}
