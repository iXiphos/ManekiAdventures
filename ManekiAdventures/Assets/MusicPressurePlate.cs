using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPressurePlate : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            gameObject.GetComponentInChildren<BambooPipe>().PlaySoundBasedOnSize();
            AudioManager am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            am.Play("pressure-plate");
        }
    }
}
