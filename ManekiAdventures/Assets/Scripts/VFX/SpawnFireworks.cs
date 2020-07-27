using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFireworks : MonoBehaviour
{
    public GameObject fireworkPrefab;
    public GameObject lantern1;
    public GameObject lantern2;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SoccerBall")
        {
            StartCoroutine(FireworkCoroutine());
        }
    }

    IEnumerator FireworkCoroutine()
    {
        GameObject fireworks1 = Instantiate(fireworkPrefab);
        fireworks1.transform.position = lantern1.transform.position;
        GameObject fireworks2 = Instantiate(fireworkPrefab);
        fireworks2.transform.position = lantern2.transform.position;

        yield return new WaitForSeconds(10f);

        Destroy(fireworks1);
        Destroy(fireworks2);
    }
}
