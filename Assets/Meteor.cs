using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    private float speed;
    private Transform cameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        speed = Random.Range(0, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Time.deltaTime * speed * Vector3.down;
        if (transform.position.y < cameraTransform.position.y - 10){
            Destroy(gameObject);
        }
    }
}
