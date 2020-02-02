using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePing : MonoBehaviour
{
    public float epsilon = 1;
    bool grappled = false;
    bool jumped = false;
    Gun player;
    MoveTrail grapple;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<Gun>();
        grapple = GameObject.FindObjectOfType<MoveTrail>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!jumped && dist(player.transform.position) < epsilon)
        {
            grappled = true;
            jumped = true;
            player.jumpOn(this.transform);
            return;
        }
        else if (!grappled && dist(grapple.transform.position) < epsilon)
        {
            grappled = true;
            grapple.latchOn(transform);
            return;
        }
    }

    float dist (Vector3 pos)
    {
        return (pos - transform.position).magnitude;
    }
}
