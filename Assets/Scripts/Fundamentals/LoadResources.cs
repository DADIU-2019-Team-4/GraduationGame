using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.IO;

public class LoadResources : MonoBehaviour
{
    private AssetBundle bundleRequest;
    private UnityWebRequest request;
    public string link;
    public string objectName;
    public AssetsInformation[] assetList;

    private void Start()
    {
        StartCoroutine(LoadAsset());
    }
    IEnumerator  LoadAsset()
    {
        Time.timeScale = 0;
        for (int i = 0; i <= assetList.Length - 1; i++)
            {
                request = UnityWebRequestAssetBundle.GetAssetBundle(link);
                yield return request.SendWebRequest();
                bundleRequest = DownloadHandlerAssetBundle.GetContent(request);
                if (bundleRequest != null)
                {
                    GameObject platform = bundleRequest.LoadAsset<GameObject>(objectName);
                    Instantiate(platform);
                }
                else
                    Debug.Log("Asset bundle wasn't received");    
        }
        Time.timeScale = 1;
    }
}
