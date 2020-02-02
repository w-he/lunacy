using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scroll : MonoBehaviour
{
    public float speed = 0.5f;
    public Transform target;

    private GameObject nextBackground;
    private Vector3 nextBackgroundPos;
    public GameObject meteor;
    public GameObject currentBackground;
    private float spriteLength;

    private float nextMeteor;
    // Start is called before the first frame update
    void Start()
    {
        nextMeteor = Time.time + 1;
        SpriteRenderer spriteRenderer = currentBackground.GetComponent<SpriteRenderer>();
        spriteLength = spriteRenderer.sprite.bounds.size.y;
        nextBackgroundPos = new Vector3(0, spriteLength * 2, 0);
        nextBackground = Instantiate(currentBackground, nextBackgroundPos, Quaternion.identity);

        Vector2 spawnLocation = new Vector2(0, 0);
        for (int i = 0; i < 2; i++)
        {
            spawnLocation.x = Random.Range(-6, 6);
            spawnLocation.y += Random.Range(2, 5);
            Instantiate(meteor, spawnLocation, Quaternion.identity);
        }
    }

    private void Update()
    {
        transform.position += Time.deltaTime * speed * Vector3.up;
        if (nextBackground.transform.position.y < gameObject.transform.position.y)
        {
            Destroy(currentBackground);
            currentBackground = nextBackground;
            nextBackgroundPos = new Vector3(0, currentBackground.transform.position.y + spriteLength * 2f, 0);
            nextBackground = Instantiate(currentBackground, nextBackgroundPos, Quaternion.identity);
        }

        if (Time.time > nextMeteor)
        {
            nextMeteor += Random.Range(0.5f, 1.5f);
            Instantiate(meteor, new Vector2(Random.Range(-6, 6), transform.position.y + 10), Quaternion.identity);
        }

        if (target.transform.position.y < gameObject.transform.position.y - 10)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
 
    }

    // Update is called once per frame
    void LateUpdate()
    {
       
        if (target.position.y > transform.position.y)
        {
            Vector3 newPos = new Vector3(transform.position.x, target.position.y,-10);
            transform.position = newPos;
        }
    }
}
