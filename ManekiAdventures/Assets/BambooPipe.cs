using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BambooPipe : MonoBehaviour
{
    //pipes can play one of 4 notes: D, F#, A, D
    // if the player tries to change the size past these bounds, the pipes will not change size.
    float pipeSizeIncrease = 1.3f;
    float pipeSizeDecrease = 1.3f;
    static float baseSize = 3f;
    public float currSize = 3f;

    public void PlaySoundBasedOnSize()
    {
        float expSize = currSize / baseSize;
        for(int i = 0; i < 4; i++)
        {
            // this rounds to 2 decimal places
            expSize = Mathf.Ceil(expSize * 100) / 100;
            float calculatedSize = Mathf.Ceil(Mathf.Pow(pipeSizeIncrease, i) * 100) / 100;
            if (expSize == calculatedSize)
            {
                AudioManager am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
                // set pitch depending on what size it is
                //Debug.Log(i + ": " + (1f / Mathf.Pow(2f, 4f - (i+1)))+1);
                //am.Play("pan-flute-d", 0.5f, 1f/((1f/Mathf.Pow(2f, 4f-i))+1f), false);
                if(i == 0)
                    am.Play("pan-flute-d", 0.5f, 1f, false);
                else
                    am.Play("pan-flute-d", 0.5f, (1f / Mathf.Pow(2f, 4f - (i+1))+1), false);
                break;
            }
        }
        
    }

    // THIS IS A MODIFIED VERSION OF POTION.CS'S ONTRIGGERENTER
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Potion")
        {
            GameObject potion = collision.gameObject;
            StartCoroutine(Explosion(potion.GetComponent<Potion>()));

            // play particles
            GameObject explosionGame = Instantiate(potion.GetComponent<Potion>().explosion, potion.transform.position, potion.transform.rotation);
            Destroy(explosionGame, 2.0f);
        }
    }

    IEnumerator Explosion(Potion potion)
    {
        yield return new WaitForSeconds(0.1f);
        switch (potion.pAttribute)
        {
            case attribute.Size:
                //Change the Size of the Object
                if (potion.pDiscriptor == discriptor.Increases)
                {
                    gameObject.transform.localScale *= pipeSizeIncrease;
                    Debug.Log("Grow");
                    Destroy(potion.gameObject);
                    currSize *= pipeSizeIncrease;
                }
                else if (potion.pDiscriptor == discriptor.Decreases)
                {
                    gameObject.transform.localScale /= pipeSizeDecrease;
                    Debug.Log("Shrink");
                    Destroy(potion.gameObject);
                    currSize /= pipeSizeIncrease;
                }
                break;
        }
    }
}
