using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DELETE_LATERtutorialTxt : MonoBehaviour
{
    public TextMeshProUGUI text;

    //public string[] texts;
    public int textForThis;

    public bool canChangeText = true;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (canChangeText)
            {
                text.text = "Drag back and hold to charge your dash \n\n When the arrow gets red, release to dash through objects to burn them"; //texts[textForThis];
            }
            else
                text.text = "";
        }
    }
}
