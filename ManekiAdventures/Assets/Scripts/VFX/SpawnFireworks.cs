using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFireworks : MonoBehaviour
{
    public GameObject fireworkPrefab;
    public GameObject lantern1;
    public GameObject lantern2;
    public GameObject respawn;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SoccerBall")
        {
            StartCoroutine(FireworkCoroutine(other));
        }
    }

    IEnumerator FireworkCoroutine(Collider other)
    {
        GameObject fireworks1 = Instantiate(fireworkPrefab);
        fireworks1.transform.position = lantern1.transform.position;
        GameObject fireworks2 = Instantiate(fireworkPrefab);
        fireworks2.transform.position = lantern2.transform.position;

        yield return new WaitForSeconds(5f);

        other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        other.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        other.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        other.gameObject.transform.position = respawn.transform.position;

        yield return new WaitForSeconds(5f);

        Destroy(fireworks1);
        Destroy(fireworks2);
    }
}
