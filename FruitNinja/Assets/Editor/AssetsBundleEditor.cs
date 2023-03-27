using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class AssetsBundleEditor : MonoBehaviour
{
    [MenuItem("AssetsBundle/Build_Win64")]
    public static void BuildAssetsBundle_Win64()
    {
        BuildAssetsBundle(BuildTarget.StandaloneWindows64);
    }
    private static void BuildAssetsBundle(BuildTarget target)
    {
        string packagePath = Application.streamingAssetsPath;
        if (packagePath.Length <= 0 && !Directory.Exists(packagePath))
        {
            return;
        }
        BuildPipeline.BuildAssetBundles(packagePath, BuildAssetBundleOptions.UncompressedAssetBundle,target);

    }
}
