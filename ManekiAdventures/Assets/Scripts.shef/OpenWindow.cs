using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWindow : MonoBehaviour
{
    public GameObject Window;

    GameObject focusObj;

    public void WindowOpener()
    {
        if (Window != null)
        {
            bool isActive = Window.activeSelf;

            Window.SetActive(!isActive);

            // set player animations and movement locks
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Movement>().canMove = isActive;
            player.GetComponent<Animator>().SetBool("isCrafting", !isActive);

            if (!isActive)
            {
                focusObj = Instantiate(DialogueEventController.dofController, GameObject.Find("RU").transform);
                focusObj.GetComponent<DOFControl>().additionalOffset = new Vector3(5, -1, -1);
                focusObj.GetComponent<DOFControl>().ToggleFocusCamera();
                Vector3 dc = GameObject.Find("DynamicCamera3D").transform.position;
                player.transform.LookAt(new Vector3(dc.x, player.transform.position.y, dc.z));
            }
            else
            {
                StartCoroutine(PlayCraftingExitThenDestroy());
            }
        }
    }

    IEnumerator PlayCraftingExitThenDestroy()
    {
        focusObj.GetComponent<DOFControl>().additionalOffset = new Vector3(0, -1, 0);
        yield return new WaitForSeconds(1.5f);
        focusObj.GetComponent<DOFControl>().ToggleFocusCamera();
        Destroy(focusObj, 1f);
    }
}
