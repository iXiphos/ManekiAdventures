using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float ballForce = 1f;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "InvisWalls")
        {
            Vector3 kickVector = gameObject.transform.position - collision.gameObject.transform.position;
            kickVector = new Vector3(kickVector.x, 2f, kickVector.z);
            gameObject.GetComponent<Rigidbody>().AddForce((kickVector).normalized * ballForce);
        }
    }
}
