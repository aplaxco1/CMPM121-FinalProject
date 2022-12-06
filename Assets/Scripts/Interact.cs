using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public class Interact : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] public GameObject numText;
    public int total = -1;

    [Header("Object")]
    public float floatStrength = 0.25f; 
    float originalY;


    void Start()
    {
        numText.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = (total).ToString();
        this.originalY = this.transform.position.y + 0.5f;
    }


    void Update()
    {
        transform.position = new Vector3(transform.position.x, originalY + ((float)Math.Sin(Time.time) * floatStrength), transform.position.z);
        transform.Rotate(0,1,0);
    }


    private void UpdateItems(){
        string totalStr = numText.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
        int totalNum;
        int.TryParse(totalStr, out totalNum);
        numText.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = (totalNum - 1).ToString();

    }


    private void OnCollisionEnter(Collision other) {
        //Play sound
        UpdateItems();
        Destroy(gameObject);
    }


}
