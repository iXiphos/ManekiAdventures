using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuViewSwitcher : MonoBehaviour
{
    public List<GameObject> views;
    public string defaultView = "MainView";

    // Start is called before the first frame update
    void Start()
    {
        ShowView(defaultView);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowView(string name)
    {
        // go back to the main view
        foreach (GameObject view in views)
        {
            if (view.name == name)
            {
                view.SetActive(true);
            }
            else
            {
                view.SetActive(false);
            }
        }
    }
}
