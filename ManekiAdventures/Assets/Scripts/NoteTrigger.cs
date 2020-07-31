using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteTrigger : MonoBehaviour
{
    public GameObject scrollEventController;
    public string imgFileName;
    public string eventAfterFileName;
    bool inEvent = false;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, gameObject.transform.position + new Vector3(0, Mathf.Sin(Time.time) * 0.1f), Time.deltaTime * 2f);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if(!inEvent && Input.GetKeyUp(KeyCode.E))
            {
                inEvent = true;
                GameObject.Find("UICanvases").GetComponent<CanvasManager>().SetGamestateByCanvasName("ScrollCanvas");
                scrollEventController.GetComponent<ScrollEventController>().DisplayScroll(imgFileName);
                if(eventAfterFileName != "")
                {
                    if (eventAfterFileName.Contains("UNIQUE"))
                    {
                        if (StoryEventHandler.uniqueEventTracker.ContainsKey(eventAfterFileName))
                            return;
                        else
                        {
                            StoryEventHandler.uniqueEventTracker[eventAfterFileName] = true;
                            StartCoroutine(FireDialogueAfterExit());
                        }
                    }
                    else
                    {
                        StartCoroutine(FireDialogueAfterExit());
                    }
                }
            }
        }
    }

    IEnumerator FireDialogueAfterExit()
    {
        bool hasFired = false;
        while(!hasFired)
        {
            yield return new WaitForSeconds(0.5f);
            if (CanvasManager.canvasState != "ScrollCanvas")
            {
                hasFired = true;
                inEvent = false;
                GameObject.Find("DialogueEventController").GetComponent<DialogueEventController>().ExecuteEvent(eventAfterFileName);
            }
        }
    }
}
