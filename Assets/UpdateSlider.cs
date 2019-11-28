using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSlider : MonoBehaviour
{

    public string Key;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Slider>().value = PlayerPrefs.GetFloat(Key);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
