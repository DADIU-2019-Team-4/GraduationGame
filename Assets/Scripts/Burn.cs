using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Burn : MonoBehaviour
{
    public GameObject _optionsMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("B");
            StartCoroutine(BurnOptionsMenu(1));
        }
    }
	public IEnumerator BurnOptionsMenu(float duration)
	{
		float dissolveStartValue = 0f;
		float timer = 0;

		while (timer < duration)
		{
			float burnValue = _optionsMenu.transform.Find("Burn").GetComponent<Image>().material.GetFloat("_Float0");

			burnValue = Mathf.Lerp(1f, dissolveStartValue, timer / duration);
			_optionsMenu.transform.Find("Burn").GetComponent<Image>().material.SetFloat("_Float0", burnValue);

			timer += Time.unscaledDeltaTime;
			yield return null;
		}

		_optionsMenu.transform.Find("Burn").GetComponent<Image>().material.SetFloat("_Float0", dissolveStartValue);



		//yield return new WaitForSeconds(duration);
	}
}
