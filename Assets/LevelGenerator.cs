using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject meteorPrefab;
    public float width;
    public float minY = 2f;
    public float maxY = 7f;
    public float minX = -6f;
    public float maxX = 6f;
    private float startY = -1;

    public int numMeteors = 3;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 spawnLocation = new Vector2(0, startY);
        for (int i = 0; i < numMeteors; i++)
        {
            spawnLocation.x = Random.Range(minX, maxX);
            spawnLocation.y += Random.Range(minY, maxY);
            Instantiate(meteorPrefab, spawnLocation, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
