using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuCharacter : MonoBehaviour
{
    DialogueEventController dialogueEventController;
    GameObject kiki;
    public float interactDistance = 4f;
    public Vector3 followDistance = new Vector3(-2f, 2f, 2f);
    public float bobbingHeight = 0.5f;
    public float bobbingSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        dialogueEventController = GameObject.Find("DialogueEventController").GetComponent<DialogueEventController>();
        kiki = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        FireflyFollow();


        // SAMPLE
        /*
        if (!DialogueEvent.inDialogue)
        {
            if (Input.GetKeyUp(KeyCode.Space) && (Vector3.Distance(gameObject.transform.position, kiki.transform.position) <= interactDistance))
            {
                Dictionary<string, string> vars = new Dictionary<string, string>();
                vars.Add("item", "glowing purple flower");
                dialogueEventController.ExecuteEventWithVars("SAMPLE_DIALOGUE", vars);
            }
        }*/
    }

    void FireflyFollow()
    {
        // movement (random soft movement to simulate a firefly)
        Vector3 fireflyMovement = new Vector3(0, bobbingHeight * Mathf.Cos(Time.time * bobbingSpeed), 0) + new Vector3((bobbingHeight / 2) * Mathf.Sin(Time.time * bobbingSpeed * 2), 0, (bobbingHeight / 2) * Mathf.Sin(Time.time * bobbingSpeed));
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, fireflyMovement + kiki.transform.position + followDistance, Time.deltaTime);

        // rotation (look at kiki)
        gameObject.transform.LookAt(kiki.transform);
    }
}
