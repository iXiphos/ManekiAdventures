using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwap : MonoBehaviour
{
    public void changeScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
