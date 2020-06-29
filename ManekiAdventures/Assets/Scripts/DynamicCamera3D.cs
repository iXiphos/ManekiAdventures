//Michelle Nie
// NOTE THIS IS ONLY FOR 2D (6/18) AND HAS NOT BEEN ADJUSTED FOR 3D YET

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera3D : MonoBehaviour
{
    /* Purpose of this script:
     * Smoothly positions the camera SMOOTHLY between two objects
     * Resizes the camera to appropriately accomodate for both objects
     */

    [SerializeField] private GameObject player; // player
    public GameObject focusObj; // obj to focus

    public float cameraOffsetX = -8f;
    public float cameraOffsetY = 10f;
    public float cameraOffsetZ = -8f;

    [SerializeField] private float minCameraSize = 4f;
    [SerializeField] private float normalCameraSize = 5f;
    //[SerializeField] private float maxCameraSize = 8f;

    [SerializeField] private float lerpSpeed = 1f;

    private float distanceBetweenPlayers; //distance between player and focus

    private void Start()
    {
        //find player
        player = GameObject.FindGameObjectWithTag("Player");
        // reset focus
        focusObj = null;

    }

    // Update is called once per frame
    void Update()
    {
        if (focusObj != null)
        {
            CalculateDistanceBetweenPlayers();
            CameraFollow2Players();
        }
        else
        {
            // if nothing is focused, do normal camera movement
            CameraFollowPlayer();
        }
    }

    void CameraFollowPlayer()
    {
        // lerp towards player
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(player.transform.position.x + cameraOffsetX, player.transform.position.y + cameraOffsetY, player.transform.position.z + cameraOffsetZ), Time.deltaTime * lerpSpeed);
        //gameObject.transform.rotation;

        // reset camera size to normal 
        gameObject.GetComponent<Camera>().orthographicSize = Mathf.Lerp(gameObject.GetComponent<Camera>().orthographicSize, normalCameraSize, Time.deltaTime * lerpSpeed);
    }

    void CalculateDistanceBetweenPlayers()
    {
        //Collect positions of players
        Vector3 playerPos = player.transform.position;
        Vector3 focusPos = focusObj.transform.position;

        //Get distance in Vector3
        Vector3 displacement = focusPos - playerPos;

        //Use Pythagorean theorem to calculate linear distance
        distanceBetweenPlayers = Mathf.Sqrt(Mathf.Pow(displacement.x, 2) + Mathf.Pow(displacement.z, 2)); //square root of (x^2 + y^2)

        // Uncomment for debug
        //Debug.Log(distanceBetweenPlayers);
    }

    void CameraFollow2Players()
    {
        Vector3 distanceBetweenPlayersV3 = GetVector3DistanceBetweenPlayers();

        //update position to be the midpoint between the two players----------
        if (distanceBetweenPlayers > 0)   //positive = player1 on the right
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3((player.transform.position.x - distanceBetweenPlayersV3.x / 2) + cameraOffsetX, player.transform.position.y + cameraOffsetY, (player.transform.position.z - distanceBetweenPlayersV3.z / 2) + cameraOffsetZ), Time.deltaTime * lerpSpeed);
        else                                                    //negative = player2 on the right
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3((focusObj.transform.position.x - distanceBetweenPlayersV3.x / 2) + cameraOffsetX, focusObj.transform.position.y + cameraOffsetY, (focusObj.transform.position.z - distanceBetweenPlayersV3.z / 2) + cameraOffsetZ), Time.deltaTime * lerpSpeed);


        //update scale--------------------------------------------------------
        //min size
        if (distanceBetweenPlayers < minCameraSize)
        {
            //camera size: current size --> min size
            gameObject.GetComponent<Camera>().orthographicSize = Mathf.Lerp(gameObject.GetComponent<Camera>().orthographicSize, minCameraSize, Time.deltaTime * lerpSpeed);
        }
        //in between size
        else if (distanceBetweenPlayers > minCameraSize /*&& distanceBetweenPlayers < maxCameraSize*/)
        {
            //camera size: current size --> minimum size + position difference / 5
            gameObject.GetComponent<Camera>().orthographicSize = Mathf.Lerp(gameObject.GetComponent<Camera>().orthographicSize, minCameraSize + (distanceBetweenPlayers / 5), Time.deltaTime * lerpSpeed);
        }
    }

    Vector3 GetVector3DistanceBetweenPlayers()
    {
        //Collect positions of players
        Vector3 playerPos = player.transform.position;
        Vector3 focusPos = focusObj.transform.position;

        //Get distance in Vector3
        Vector3 displacement = playerPos - focusPos;
        return displacement;

    }
}
