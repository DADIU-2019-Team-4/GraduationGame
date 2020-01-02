using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalizeTutorial : MonoBehaviour
{
    [TextAreaAttribute(5,10)]
    public string EnglishText;
    [TextAreaAttribute(5, 10)]
    public string DanishText;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("Language") == "English")
        {
            GetComponent<TextMeshProUGUI>().text = EnglishText;
        }
        else
        {
            GetComponent<TextMeshProUGUI>().text = DanishText;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
