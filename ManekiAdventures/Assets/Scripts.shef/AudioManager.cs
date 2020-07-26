using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds; //creates array for possible sounds

    public static AudioManager instance;

    void Awake()
    {

        if (instance == null) //these few lines looks for if an audio manager is already in the scene. if yes, do nothing, if not, create one
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);


        foreach (Sound s in sounds) // these make present the public voids in the sound script
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }


    private void Start()
    {
        Play("Theme"); //plays theme music
        Play("Ambient");
    }

    public void Play (string name) //play function for fiding the actual sound you input into audio manager
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }

        s.source.Play();
    }



}
