using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWindow : MonoBehaviour
{
    public GameObject Window;

    public void WindowOpener()
    {
        if (Window != null)
        {
            bool isActive = Window.activeSelf;

            Window.SetActive(!isActive);
        }
    }
}
