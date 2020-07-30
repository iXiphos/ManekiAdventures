using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushDetector : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude > 0.1)
            GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().isPushing = true;
       else
            GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().isPushing = false;
    }

    private void OnTriggerExit(Collider other)
    {
       // if (other.tag == "Terrain")
       // {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().isPushing = false;
       // }
    }
}
