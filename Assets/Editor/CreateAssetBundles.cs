﻿using UnityEditor;

public class CreateAssetBundles
{
    [MenuItem("Tools/Build Asset Bundle")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.StrictMode, BuildTarget.Android);
    }
}