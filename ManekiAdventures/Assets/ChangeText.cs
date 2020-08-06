using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeText : MonoBehaviour
{

    public GameObject text;
    public int radius;
    public LayerMask player;

    private void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, player);
        if (hitColliders.Length != 0)
        {
            text.SetActive(true);
        }
        else
        {
            text.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("Test");
        if (other.tag == "Player")
        {
            text.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.LogError("Teset");
        if (other.tag == "Player")
        {
            text.SetActive(false);
        }
    }

}
