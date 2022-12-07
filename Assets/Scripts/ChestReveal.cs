using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestReveal : MonoBehaviour
{
    [SerializeField] private GameObject openChest;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSFX;
    // Start is called before the first frame update
    void Start()
    {
        openChest.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] pickups;
        pickups = GameObject.FindGameObjectsWithTag("pickup");

        if(pickups.Length == 0) {
            openChest.SetActive(true);
            audioSource.PlayOneShot(openSFX);
            gameObject.SetActive(false);
        }
        
    }
}
