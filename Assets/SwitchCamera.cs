using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCamera : MonoBehaviour
{
    public GameObject[] cams;
    public int currentCam = 0;

    [SerializeField]
    private Text _camName;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < cams.Length; i++)
        {
            cams[i].SetActive(false);
        }

        _camName.text = "Current camera:\n" + cams[0].name;
        cams[0].SetActive(true);
    }

    public void NewCamera()
    {
        cams[currentCam].SetActive(false);
        if (currentCam == cams.Length - 1)
        {
            currentCam = 0;
        }
        else
        {
            currentCam += 1;
        }
        cams[currentCam].SetActive(true);
        _camName.text = "Current camera:\n" + cams[currentCam].name;
    }
}