using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI text2;

    //public string[] texts;
    public int textForThis;

    public bool canChangeText = true;


    // Start is called before the first frame update
    void Start()
    {
        LoadBaseSceneManager loadBaseSceneManager = FindObjectOfType<LoadBaseSceneManager>();
        if (loadBaseSceneManager.StoryProgression.Value != StoryProgression.EStoryProgression.At_Tutorial)
        {
            GameObject tutorial = GameObject.Find("QuickUITutorialCanvas");
            tutorial?.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (canChangeText && textForThis == 0)
            {
                text2.text = "Drag back and hold to initiate a more powerful dash. \n\n When the arrow becomes red, your dash will destroy certain objects."; //texts[textForThis];
                text.enabled = false;
                text2.enabled = true;
            }
            else if (canChangeText && textForThis == 1)
            {
                text.text = "Burning through objects restores your fire. \n\n Make sure to never let your fire run out!";
                text2.enabled = false;
                text.enabled = true;
            }
            else if (!canChangeText)
            {
                text.text = "";
                text2.text = "";
            }
        }
    }
}
