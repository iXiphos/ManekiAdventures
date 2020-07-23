using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwirlMotion : MonoBehaviour
{
    public float swirlSpeed = 5f;
    public float swirlRadius = 3f;
    public float upSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        //transform.localPosition = new Vector3(0, 0, 0);
        transform.position += new Vector3(-swirlRadius , 0, -swirlRadius);
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + Mathf.Sin(Time.time * swirlSpeed) * swirlRadius, transform.position.y + upSpeed, transform.position.z + Mathf.Cos(Time.time * swirlSpeed)*swirlRadius);
    }
}
