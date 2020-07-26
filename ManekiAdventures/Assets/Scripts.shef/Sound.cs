using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]

public class Sound
{
    public string name; //name of sound you want

    public AudioClip clip; //actual input for wav or mp3 file


    [Range(0f, 1f)]
    public float volume; //creates slider for volume

    [Range(.1f, 3f)]
    public float pitch; //creates slider for pitch

    public bool loop; //if you want sound to loop or not

    [HideInInspector]
    public AudioSource source; //creates audio source for number of sounds you have


}
