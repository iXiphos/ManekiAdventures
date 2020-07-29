using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    static public string canvasState;
    Dictionary<string, Canvas> canvases;
    // Start is called before the first frame update
    void Start()
    {
        canvases = new Dictionary<string, Canvas>();

        // get references to all the canvases
        foreach (Canvas canvas in gameObject.GetComponentsInChildren<Canvas>())
        {
            Debug.Log(canvas.name);
            canvases.Add(canvas.name, canvas);
            SetCanvasActive(canvas, false);
        }

        canvasState = "MainMenuCanvas"; // set default to main menu
        SetGamestateByCanvas(canvases[canvasState]);
    }

    void SetCanvasActive(Canvas canvas, bool isActive)
    {
        canvas.gameObject.SetActive(isActive);
    }

    public void SetGamestateByCanvasName(string canvas)
    {
        canvasState = canvas;
        SetGamestateByCanvas(canvases[canvas]);
    }

    public void SetGamestateByCanvas(Canvas canvas)
    {
        canvasState = canvas.name;
        switch (canvas.name)
        {
            case "MainMenuCanvas":
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                Vector3 dc = GameObject.Find("DynamicCamera3D").transform.position;
                player.GetComponent<Movement>().canMove = false;
                player.transform.LookAt(new Vector3(dc.x, player.transform.position.y, dc.z));
                //hide all other canvases except this one
                foreach (Canvas cv in canvases.Values)
                {
                    SetCanvasActive(cv, false);
                }
                SetCanvasActive(canvases[canvas.name], true);

                //play animation
                canvases[canvas.name].GetComponent<Animator>().SetTrigger("slideIn");
                StartCoroutine(ZoomCameraIn());

                break;
            case "PauseMenuCanvas":
                GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().canMove = false;
                // disable interacting with all canvases except this one
                // ******************************************************
                break;
            case "ScrollCanvas":

                // set this canvas overlaying dialogue/hotbar
                SetCanvasActive(canvases[canvas.name], true);

                // play anims...
                // *************

                // disable interacting with all canvases except this one
                // ******************************************************
                
                break;
            default:
            case "DialogueCanvas":
                if(DialogueEvent.inStaticDialogue)
                {
                    //hide all other canvases except this one
                    foreach (Canvas cv in canvases.Values)
                    {
                        SetCanvasActive(cv, false);
                    }
                    SetCanvasActive(canvases[canvas.name], true);
                }
                else
                {
                    goto case "HotbarCanvas";
                }
                break;
            case "HotbarCanvas": // this is inventory and crafting
                GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().canMove = true;
                StartCoroutine(ZoomCameraOut());

                // do default canvas things...
                foreach (Canvas cv in canvases.Values)
                {
                    if(cv.name == "MainMenuCanvas")
                    {
                        StartCoroutine(PlayMenuExitAnimThenDestroy(1.5f));
                    }
                    else if(cv.name != "HotbarCanvas" && cv.name != "DialogueCanvas")
                    {
                        SetCanvasActive(cv, false);
                    }
                }
                // set any default game canvases to true
                SetCanvasActive(canvases["DialogueCanvas"], true);
                SetCanvasActive(canvases["HotbarCanvas"], true);

                // play animations
                canvases["HotbarCanvas"].GetComponentInChildren<Animator>().SetTrigger("slideIn");

                break;
        }
    }

    IEnumerator PlayMenuExitAnimThenDestroy(float duration)
    {
        //play animation
        canvases["MainMenuCanvas"].GetComponent<Animator>().SetTrigger("slideOut");
        yield return new WaitForSeconds(duration);
        SetCanvasActive(canvases["MainMenuCanvas"], false);
    }

    IEnumerator ZoomCameraIn()
    {
        float timeElapsed = 0f;
        float duration = 0.4f;
        DynamicCamera3D dc = GameObject.Find("DynamicCamera3D").GetComponent<DynamicCamera3D>();
        while(timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            dc.scrollValue = 1f * Time.deltaTime;
            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }

        yield return new WaitForSecondsRealtime(duration);
        dc.scrollValue = 0f;
    }

    IEnumerator ZoomCameraOut()
    {
        float timeElapsed = 0f;
        float duration = 0.4f;
        DynamicCamera3D dc = GameObject.Find("DynamicCamera3D").GetComponent<DynamicCamera3D>();
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            dc.scrollValue = -1.5f * Time.deltaTime;
            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }

        yield return new WaitForSecondsRealtime(duration);
        dc.scrollValue = 0f;
    }
}
