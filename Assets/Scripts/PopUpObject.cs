using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpObject : MonoBehaviour
{
    public const string PopUpImageContainerName = "PopUpImageContainer";
    public const string PopUpImageName = "PopUpImage";

    public Sprite Sprite;
    GameObject _imageObject;
    private Image _imageComponent;
    private bool _isShown;

    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();

        if (!Sprite) Debug.LogWarning("A pop-up object has no image assigned to it");
        if (!canvas) Debug.LogWarning("Found no Canvas to display pop-up objects on");

        if (Sprite && canvas)
        {
            GameObject _imageContainer = canvas.transform.Find(PopUpImageContainerName).gameObject;

            if (!_imageContainer) Debug.LogWarning("Found no conatiner to display pop-up objects in");
            else
            {
                _imageObject = _imageContainer.transform.GetChild(0).gameObject;
                _imageComponent = _imageObject.GetComponent<Image>();
            }
        }
        
        _isShown = false;
    }

    public void ShowPopUp()
    {
        if (Sprite && _imageComponent && !_isShown)
        {
            Debug.Log("Spawning pop-up with name: " + Sprite.name);

            _imageComponent.sprite = Sprite;
            _imageComponent.enabled = true;
            _imageObject.gameObject.SetActive(true);
            _isShown = true;
        }
    }

    public void DestroyPopUp()
    {
        Debug.Log("Destroying pop-up");

        if (_imageObject)
        {
            _imageObject.gameObject.SetActive(false);
        }
    }
}
