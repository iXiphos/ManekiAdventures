using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRockAnim : MonoBehaviour
{
    public GameObject rock;
    public GameObject bridge;
    public GameObject particles;

    public bool hasFired = false;

    private void Awake()
    {
        //bridge.SetActive(false);
        bridge.GetComponent<Rigidbody>().Sleep();
        bridge.GetComponent<Rigidbody>().useGravity = false;
        bridge.GetComponent<Collider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!hasFired && other.tag == "Player")
        {
            StartCoroutine(AnimateAfterDelay(0.5f));
        }
    }

    static int frameCount = 0;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            frameCount++;
            if(frameCount > 60)
            {
                bridge.GetComponent<Rigidbody>().WakeUp();
                frameCount = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && hasFired)
        {
            bridge.GetComponent<Rigidbody>().Sleep();
        }
    }



    IEnumerator AnimateAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        
        rock.GetComponent<Animator>().SetTrigger("rockGrow");
        GameObject particlesObj = Instantiate(particles);
        particlesObj.transform.position = rock.transform.position;
        //bridge.SetActive(true);
        bridge.GetComponent<Collider>().enabled = true;
        bridge.GetComponent<Rigidbody>().useGravity = true;
        hasFired = true;

        yield return new WaitForSeconds(1f);
        rock.GetComponent<Animator>().enabled = false;
    }
}
