using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoadResources : MonoBehaviour
{
    private AssetBundle bundleRequest;
    private UnityWebRequest request;
    public string link;

    private void Start()
    {
        StartCoroutine(LoadAsset());
    }

    private void Update()
    {

    }
    IEnumerator  LoadAsset()
    {
        Time.timeScale = 0;
        request = UnityWebRequestAssetBundle.GetAssetBundle(link);
        yield return request.SendWebRequest();
        bundleRequest = DownloadHandlerAssetBundle.GetContent(request);
        if (bundleRequest != null)
        {
            GameObject platform = bundleRequest.LoadAsset<GameObject>("CHIMERA_model");
            Instantiate(platform);
        }
        else
            Debug.Log("Asset bundle wasn't received");
        Time.timeScale = 1;
    }
}
