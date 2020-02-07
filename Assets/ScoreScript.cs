using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public static int scoreValue = 0;
    public float subtotal = 0;
    public float timeMult;
    public float posMult;
    Text score;
    GameObject main;
    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.Find("Main Camera");
        scoreValue = 0;
        subtotal = 0;
        score = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        subtotal += timeMult * Time.deltaTime;
        scoreValue = (int)(subtotal + posMult * main.transform.position.y);
        score.text = "Score: " + scoreValue;
    }
}
