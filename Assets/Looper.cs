using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looper : MonoBehaviour
{
    public Vector2 velocity = new Vector2(0, 2);
    private float spriteLength;
    private Transform cameraTransform;

    private GameObject nextBackground;
    private Vector3 nextBackgroundPos;
    public GameObject currentBackground;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>() as SpriteRenderer;
        spriteLength = spriteRenderer.sprite.bounds.size.y;
        currentBackground = gameObject;
        nextBackgroundPos = new Vector3(0, spriteLength * 1.5f,0);
        nextBackground = Instantiate(gameObject, nextBackgroundPos, Quaternion.identity);
    }
    void Update()
    {
        if (nextBackground.transform.position.y < cameraTransform.position.y)
        {
            Destroy(currentBackground);
            currentBackground = nextBackground;
            nextBackgroundPos = new Vector3(0, currentBackground.transform.position.y + spriteLength * 1.5f, 0);
            nextBackground = Instantiate(gameObject, nextBackgroundPos, Quaternion.identity);
        }
    }
}

