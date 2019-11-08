using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AssetsInformation", menuName = "ScriptableObjects/AssetsInformation")]
public class AssetsInformation :ScriptableObject
{
    public string AssetName;
    [Tooltip("Either download link from drive (https://sites.google.com/site/gdocs2direct/) or direct link in cloud server")]
    public string AssetLink;
    public bool isDownloaded;
}
