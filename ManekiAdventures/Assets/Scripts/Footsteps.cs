using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class Footsteps : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource a;
    private bool once = false;
    private const float timerReset = 0.18f;

    void Awake()
    {
        a = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider col)
    {
        string currTag = col.tag;
        /*int currVal = -1;
        if (currTag.Length <= 2)
        {
            currVal = Int32.Parse(currTag);
        }*/

        if (!once)
        {
            if(currTag == "Terrain")
                a.PlayOneShot(clips[0]);
            once = true;
            Invoke("Reset", timerReset);
        }
    }

    private void Reset()
    {
        once = false;
    }



}