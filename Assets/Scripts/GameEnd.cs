using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour
{
    [SerializeField] private float gameEndLength;
    public RawImage black;
    private float gameEndTimer;
    private bool started = false;
    public float fadeSpeed;
    private bool fadeStarted = false;
    float fadeAmount = 0;
    Color objectColor;

    private void Update()
    {
        //Starts fading to black once the timer hits zero
        if (started && !fadeStarted)
        {
            if (gameEndTimer < 0)
            {
                fadeStarted = true;
            }
            else
            {
                gameEndTimer -= Time.deltaTime;
            }
        }
        //Quits the game once fully faded to black
        if (fadeStarted)
        {        
            objectColor = black.color;
            if(black.GetComponent<RawImage>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                black.color = objectColor;
            }
            else
            {
                Application.Quit();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Starts timer once player gets close to treasure
        if (other.tag == "Player")
        {
            
            if (!started)
            {
                started = true;
                gameEndTimer = gameEndLength;
            }
        }
    }
}
