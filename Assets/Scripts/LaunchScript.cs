using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchScript : MonoBehaviour
{
    AudioSource audioSource;
    public static bool lvlStart;
    [SerializeField] AudioClip levelStarted;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Rocket" && lvlStart==true)
        {
            audioSource.PlayOneShot(levelStarted);
            lvlStart = false;
        }
    }
}
