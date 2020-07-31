using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuTargetFollow : MonoBehaviour
{
    public Vector3 followDistance = new Vector3(-2f, 2f, 2f);

    // Update is called once per frame
    void Update()
    {
        transform.position = GameObject.FindGameObjectWithTag("Player").transform.position + followDistance;
    }
}
