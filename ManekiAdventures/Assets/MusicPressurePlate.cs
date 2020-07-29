using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPressurePlate : MonoBehaviour
{
    float baseVol = 0.1f;
    float playDistance = 30f;
    bool canPlay = true;
    private void OnCollisionEnter(Collision collision)
    {
        if(canPlay && (collision.gameObject.tag == "Player" || collision.gameObject.tag == "SoccerBall"))
        {
            float distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position);
            if(distance <= playDistance)
            {
                StartCoroutine(Cooldown());
                gameObject.GetComponentInChildren<BambooPipe>().PlaySoundBasedOnSize();
                AudioManager am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
                am.Play("pressure-plate", baseVol * (playDistance / distance), 1f, false);
            }
            
        }
    }

    IEnumerator Cooldown()
    {
        canPlay = false;
        yield return new WaitForSeconds(0.5f);
        canPlay = true;
    }
}
