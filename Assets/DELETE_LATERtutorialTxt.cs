using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DELETE_LATERtutorialTxt : MonoBehaviour
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
            tutorial.SetActive(false);
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
                text2.text = "Drag back and hold to charge your dash \n\n When the arrow gets red, release to dash through objects to burn them"; //texts[textForThis];
                text.enabled = false;
                text2.enabled = true;
            }
            else if (canChangeText && textForThis == 1)
            {
                //text2.enabled = true;
                text2.text = "Burning through objects restores your fire.";
            }
            else if (!canChangeText)
            {
                text.text = "";
                text2.text = "";
            }
        }
    }
}
