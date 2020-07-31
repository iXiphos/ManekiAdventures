using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseGame : MonoBehaviour
{
   public void QuitApplication()
    {
        //IF IN EDITOR
        //UnityEditor.EditorApplication.isPlaying = false;

        //IF IN EXECUTABLE
        Application.Quit();
    }
}

