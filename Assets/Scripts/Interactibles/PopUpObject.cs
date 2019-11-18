using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpObject : MonoBehaviour
{
    public const string PopUpCanvasTag = "PopUpCanvas";
    public Sprite Sprite;

    private bool _isShown = false;

    public void PopUp()
    {
        if (!_isShown)
        {
            GameObject canvas = GameObject.FindGameObjectWithTag(PopUpCanvasTag);

            if(!canvas) Debug.LogWarning("Unable to find Pop-up canvas to display image on");
            else
            {
                PopUpCanvas popUpCanvasComponent = canvas.GetComponent<PopUpCanvas>();
                popUpCanvasComponent.SetImage(Sprite);
                popUpCanvasComponent.ShowPopUp();
                _isShown = true;
            }
        }
    }
}
