using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBoxFollow : DialogueBox
{
    public GameObject characterToFollow;
    public Vector3 worldDisplacement = new Vector3(0, 5f, 0);

    Vector3 boxDimentions;
    Vector3 boxScale;
    float pixelsPerUnit;

    public bool isTouchingOtherBox = false;
    //float ruHeight = 4; // TO DO: implement if needed
    //float kikiHeight = 5;

        // for avoiding collisions
    float xDisp;
    float yDisp;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        // follow character
        if (characterToFollow != null)
        {
            // calculate box dimentions
            boxDimentions = gameObject.GetComponent<Image>().sprite.rect.size/4;
            boxScale = gameObject.GetComponent<RectTransform>().lossyScale;
            pixelsPerUnit = gameObject.GetComponent<Image>().sprite.pixelsPerUnit;
            boxDimentions /= pixelsPerUnit/10;
            boxDimentions = new Vector3(boxDimentions.x * boxScale.x, boxDimentions.y * boxScale.y, boxDimentions.z * boxScale.z);// * pixelsPerUnit;
            boxDimentions.x *= 3f / 2f;
            boxDimentions.y *= 2.5f;


            uiPos = mainCamera.WorldToScreenPoint(characterToFollow.transform.position + worldDisplacement);

            // check if overlap
            if (DialogueEvent.inBranch)
            {
                bool kikiWasDisplacedX = false;
                bool kikiWasDisplacedY = false;
                if (characterToFollow.name == "KIKI")
                {
                    float distanceBetweenBoxesX = uiPos.x - DialogueEvent.uiElements["RU"].GetComponent<RectTransform>().position.x;
                    if (boxDimentions.x * 2 - Mathf.Abs(distanceBetweenBoxesX) > 0)// if it's too close, shift it back
                    {
                        xDisp = boxDimentions.x * 2 - distanceBetweenBoxesX;
                        uiPos.x += xDisp * 1/3;
                        kikiWasDisplacedX = true;
                    }
                    else
                        kikiWasDisplacedX = false;
                    float distanceBetweenBoxesY = uiPos.y - DialogueEvent.uiElements["RU"].GetComponent<RectTransform>().position.y;
                    if (boxDimentions.y*2 - Mathf.Abs(distanceBetweenBoxesY) > 0) // if it's too close, shift it back
                    {
                        yDisp = boxDimentions.y * 2 - Mathf.Abs(distanceBetweenBoxesY);
                        uiPos.y -= yDisp;
                        kikiWasDisplacedY = true;
                    }
                    else
                        kikiWasDisplacedY = true;
                }

                if (characterToFollow.name == "RU")
                {
                    if (kikiWasDisplacedX)// if it's too close, shift it back
                        uiPos.x -= xDisp * 2/3;
                    if (kikiWasDisplacedY) // if it's too close, shift it back
                        uiPos.y += yDisp;
                }
            }

            // check if off screen
            if (uiPos.x > Screen.width - boxDimentions.x) // if offscreen (left/right)
            {
                uiPos = new Vector3(Screen.width - boxDimentions.x, uiPos.y, uiPos.z);
            }
            else if (uiPos.x < 0 + boxDimentions.x)
            {
                uiPos = new Vector3(boxDimentions.x, uiPos.y, uiPos.z);
            }
            if (uiPos.y > Screen.height - boxDimentions.y) // if offscreen (left/right)
            {
                uiPos = new Vector3(uiPos.x, Screen.height - boxDimentions.y, uiPos.z);
            }
            else if (uiPos.y < 0 + boxDimentions.y)
            {
                uiPos = new Vector3(uiPos.x, boxDimentions.y, uiPos.z);
            }

            // set position
            gameObject.GetComponent<RectTransform>().position = Vector3.Lerp(gameObject.GetComponent<RectTransform>().position, uiPos, Time.deltaTime * 4f);

        }

        if (textUI != null)
        {
            // assign text
            textUI.text = currLine;
        }
    }
    /*
    private void Update()
    {
        // NOTE THAT THIS IS HARDCODED AND SHOULD BE FIXED!!!!!!!!!!1 ********************
        //GameObject ru = GameObject.Find("RU");
        //GameObject kiki = GameObject.Find("KIKI");
        //displacement = new Vector3(0, Vector3.Distance(ru.transform.position, kiki.transform.position), 0);
        worldDisplacement = new Vector3(0, 4.5f, 0);
    }*/
}
