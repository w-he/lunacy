using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public static int counter = 0;
    public float speed;
    private Transform cameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        speed = Random.Range(0, 2f);
        counter++;
        Debug.Log(counter);
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
