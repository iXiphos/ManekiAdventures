using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPressurePlate : MonoBehaviour
{
    bool canPlay = true;
    private void OnCollisionEnter(Collision collision)
    {
        if(canPlay && (collision.gameObject.tag == "Player" || collision.gameObject.tag == "SoccerBall"))
        {
            StartCoroutine(Cooldown());
            gameObject.GetComponentInChildren<BambooPipe>().PlaySoundBasedOnSize();
            AudioManager am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            am.Play("pressure-plate");
        }
    }

    IEnumerator Cooldown()
    {
        canPlay = false;
        yield return new WaitForSeconds(0.5f);
        canPlay = true;
    }
}
