using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcornReset : MonoBehaviour
{
    public GameObject acornRespawnPoint;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "SoccerBall")
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            other.gameObject.transform.rotation = acornRespawnPoint.transform.rotation;
            other.gameObject.transform.position = acornRespawnPoint.transform.position;
        }
    }
}
