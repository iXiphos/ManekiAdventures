using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollEventController : MonoBehaviour
{
    public static List<ScrollData> scrollsInteracted = new List<ScrollData>();

    public GameObject nextArrowPrefab;
    public GameObject prevArrowPrefab;
    public GameObject imagePrefab;
    //public GameObject textPrefab;
    public GameObject scrollsParent;
    public GameObject warnText;

    bool inScrollEvent = false;

    // for multi events
    GameObject nextArrow;
    GameObject prevArrow;
    bool inMultiScrollEvent = false;
    int currScrollInMulti;
    int totalNumScrollsInMulti = 0;
    GameObject multiParentObj;

    public struct ScrollData
    {
        public string textFile;
        public string imgFile;
        public string formattedText;
    }

    private void Update()
    {
        // if in a multi event, hide arrows as needed
        if(inMultiScrollEvent)
        {
            if(totalNumScrollsInMulti == 0)
            {
                prevArrow.SetActive(false);
                nextArrow.SetActive(false);
            }
            else
            {
                if (currScrollInMulti == 0)
                    prevArrow.SetActive(false);
                else
                    prevArrow.SetActive(true);

                if (currScrollInMulti == totalNumScrollsInMulti - 1)
                    nextArrow.SetActive(false);
                else
                    nextArrow.SetActive(true);
            }
        }
    }

    public void ExecuteScrollEventByString(string imgFileName)
    {
        GameObject canvasManager = GameObject.Find("UICanvases");
        if (imgFileName == "ALL")
        {
            // multi
            canvasManager.GetComponent<CanvasManager>().SetGamestateByCanvasName("ScrollCanvas");
            gameObject.GetComponent<ScrollEventController>().DisplayMultiple(ScrollEventController.scrollsInteracted);
        }
        //----------------------------------------
        else
        {
            // execute image only
            canvasManager.GetComponent<CanvasManager>().SetGamestateByCanvasName("ScrollCanvas");
            gameObject.GetComponent<ScrollEventController>().DisplayScroll(imgFileName);
        }
    }

    public void ExitScrollUI()
    {
        // hide any scroll UI --> Handled by canvas manager
        
        // destroy the scrolls inside the UI
        foreach (Transform obj in scrollsParent.GetComponentsInChildren<Transform>())
        {
            if(obj.name != "ScrollsParent")
                Destroy(obj.gameObject);
        }

        totalNumScrollsInMulti = 0;

        inMultiScrollEvent = false;
        inScrollEvent = false;
        warnText.SetActive(false);
    }

    public void DisplayMultiple(List<ScrollData> scrolls) // displays multiple scrollData with arrows to switch between
    {
        inScrollEvent = true;
        inMultiScrollEvent = true;

        // spawn arrow with scroll UI
        nextArrow = Instantiate(nextArrowPrefab, scrollsParent.transform);
        nextArrow.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0.5f);
        nextArrow.GetComponent<RectTransform>().anchorMin = new Vector2(1f, 0.5f);
        nextArrow.GetComponent<RectTransform>().localPosition = new Vector3(300, 0, 0);

        prevArrow = Instantiate(prevArrowPrefab, scrollsParent.transform);
        prevArrow.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 0.5f);
        prevArrow.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0.5f);
        prevArrow.GetComponent<RectTransform>().localPosition = new Vector3(-300, 0, 0);

        nextArrow.GetComponent<Button>().onClick.AddListener(delegate () { NextScroll(); });
        prevArrow.GetComponent<Button>().onClick.AddListener(delegate () { PrevScroll(); });
        nextArrow.SetActive(false);
        prevArrow.SetActive(false);

        // spawn parent (so all scrolls can be moved at once)
        multiParentObj = Instantiate(new GameObject(), scrollsParent.transform);
        multiParentObj.name = "MultiScrollParent";
        multiParentObj.AddComponent<RectTransform>();

        // spawn all of them under a parent (each one is one screen size offset)
        totalNumScrollsInMulti = 0;
        foreach (ScrollData scroll in scrolls)
        {
            ScrollData scrollToAdd = new ScrollData
            {
                textFile = scroll.textFile,
                imgFile = scroll.imgFile,
                formattedText = scroll.formattedText,
            };
            bool hasAdded = false;
            if (scroll.textFile == scrollToAdd.textFile &&
                scroll.imgFile == scrollToAdd.imgFile &&
                scroll.formattedText == scrollToAdd.formattedText)
            {
                hasAdded = true;
            }
            if(!hasAdded)
                scrollsInteracted.Add(scrollToAdd); // add scroll if it hasn't been interacted with

            // retrieve sprite
            Sprite img = Resources.Load<Sprite>("Scrolls\\Images\\" + scroll.imgFile);

            // spawn the UI item in scene
            GameObject scrollImage = Instantiate(imagePrefab, multiParentObj.transform);
            scrollImage.name = scroll.imgFile;

            scrollImage.GetComponent<Image>().sprite = img;
            scrollImage.GetComponent<Image>().SetNativeSize();

            scrollImage.GetComponent<RectTransform>().localScale = new Vector3(0.37f, 0.37f, 0.37f); // scale to fit
            scrollImage.GetComponent<RectTransform>().localPosition = new Vector3(Screen.width * totalNumScrollsInMulti, 0, 0); // center the image with displacement
            totalNumScrollsInMulti++;
        }

        if (totalNumScrollsInMulti == 0)
        {
            warnText.SetActive(true);
        }

        // keep track of scroll numbers
        currScrollInMulti = 0;
        
    }

    public void DisplayScroll(string imgFile)
    {
        inScrollEvent = true;

        Debug.Log("Executing scroll event: " + imgFile);

        // retrieve sprite
        Sprite img = Resources.Load<Sprite>("Scrolls\\Images\\" + imgFile);

        // spawn the UI item in scene
        GameObject scrollImage = Instantiate(imagePrefab, scrollsParent.transform);
        scrollImage.name = imgFile;

        scrollImage.GetComponent<Image>().sprite = img;
        scrollImage.GetComponent<Image>().SetNativeSize();

        scrollImage.GetComponent<RectTransform>().localScale = new Vector3(0.37f, 0.37f, 0.37f); // scale to fit
        scrollImage.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0); // center the image

        // after displaying, keep track of what you've displayed
        // check to make sure it isn't already added
        ScrollData scrollToAdd = new ScrollData
        {
            textFile = "",
            imgFile = imgFile,
            formattedText = ""
        };

        bool hasAdded = false;
        foreach (ScrollData scroll in scrollsInteracted)
        {
            if (scroll.textFile == scrollToAdd.textFile &&
                scroll.imgFile == scrollToAdd.imgFile &&
                scroll.formattedText == scrollToAdd.formattedText
                )
            {
                hasAdded = true;
            }
        }

        if(!hasAdded)
            scrollsInteracted.Add(scrollToAdd);
    }

    public void DisplayScroll(string imgFile, string textFile)
    {
        inScrollEvent = true;

        Debug.Log("Executing scroll event: " + imgFile + " with " + textFile);

        // retrieve sprite and text
        Sprite img = Resources.Load<Sprite>("Scrolls\\Images\\" + imgFile);
        string text = Resources.Load<TextAsset>("Scrolls\\text" + textFile).text;

        // **** ADD TEXT FORMATTING LATER......

        scrollsInteracted.Add(new ScrollData
        {
            textFile = text,
            imgFile = imgFile,
            formattedText = text,
        });
    }

    public static void NextScroll()
    {
        ScrollEventController scrollEventController = GameObject.Find("ScrollCanvas").GetComponent<ScrollEventController>();
        if (scrollEventController.currScrollInMulti < scrollEventController.totalNumScrollsInMulti)
        {
            scrollEventController.currScrollInMulti++; //iterate

            // move the UI to display the next scroll
            scrollEventController.multiParentObj.GetComponent<RectTransform>().localPosition -= new Vector3(Screen.width, 0, 0);
        }
    }

    public static void PrevScroll()
    {
        ScrollEventController scrollEventController = GameObject.Find("ScrollCanvas").GetComponent<ScrollEventController>();
        if (scrollEventController.currScrollInMulti >0)
        {
            scrollEventController.currScrollInMulti--; //backwards iterate

            // move the UI to display the prev scroll
            scrollEventController.multiParentObj.GetComponent<RectTransform>().localPosition += new Vector3(Screen.width, 0, 0);
        }
    }
}
