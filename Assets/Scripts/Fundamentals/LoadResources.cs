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
                request = UnityWebRequestAssetBundle.GetAssetBundle(assetList[i].AssetLink,1,0);
                yield return request.SendWebRequest();
                bundleRequest = DownloadHandlerAssetBundle.GetContent(request);
                if (bundleRequest != null)
                {
                    GameObject platform = bundleRequest.LoadAsset<GameObject>(assetList[i].SceneAssetName);
                    Instantiate(platform);
                }
                else
                    Debug.Log("Asset bundle wasn't received");    
        }
        Time.timeScale = 1;
    }
}
