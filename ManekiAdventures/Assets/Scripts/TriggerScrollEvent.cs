using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScrollEvent : MonoBehaviour
{
    public string imgFileName = "";
    public string textFileName = ""; //Optional
    GameObject canvasManager;
    GameObject scrollEventController;

    private void Start()
    {
        canvasManager = GameObject.Find("UICanvases");
        scrollEventController = GameObject.Find("ScrollCanvas");
    }

    private void OnTriggerStay(Collider other)
    {
        if(Input.GetKeyUp(KeyCode.E))
        {
            //DEBUG, REMOVE LATER---------------------
            if(imgFileName == "ALL")
            {
                // multi
                canvasManager.GetComponent<CanvasManager>().SetGamestateByCanvasName("ScrollCanvas");
                scrollEventController.GetComponent<ScrollEventController>().DisplayMultiple(ScrollEventController.scrollsInteracted);
            }
            //----------------------------------------
            else if(textFileName == "")
            {
                // execute image only
                canvasManager.GetComponent<CanvasManager>().SetGamestateByCanvasName("ScrollCanvas");
                scrollEventController.GetComponent<ScrollEventController>().DisplayScroll(imgFileName);
            }
            else
            {
                //execute text and image if text is provided
                canvasManager.GetComponent<CanvasManager>().SetGamestateByCanvasName("ScrollCanvas");
                scrollEventController.GetComponent<ScrollEventController>().DisplayScroll(imgFileName, textFileName);
            }
        }
    }
}
