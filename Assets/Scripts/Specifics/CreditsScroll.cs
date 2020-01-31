using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsScroll : MonoBehaviour
{
    private RectTransform rectTransform;
    public float Speed;
    public float StartPos;
    public float EndPos;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition3D = new Vector3(0, StartPos, 0);
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.anchoredPosition3D = new Vector3(0, rectTransform.anchoredPosition3D.y+ (Speed*Time.deltaTime), 0);

        if (rectTransform.anchoredPosition3D.y > EndPos)
        {
            GoToMainMenu();
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
