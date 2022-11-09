using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class AssetsBundleMgr : MonoBehaviour
{
    public static void DownLoadAssets(string path,string name,Action<object>callBack)
    {
        new Task(GetAssetsBundle(path,name,callBack));
    }
    private static IEnumerator GetAssetsBundle(string path,string name,Action<object> callBack)
    {
        //请求，发送，下载，加载，等待完成，克隆，卸载
        var uwr = UnityWebRequestAssetBundle.GetAssetBundle(@"file:///" + path + name);
        yield return uwr.SendWebRequest();
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
        var loadAsset = bundle.LoadAllAssetsAsync();
        yield return loadAsset;
        var prefab = loadAsset.asset;
        var go = Instantiate(prefab);
        callBack(go);
        bundle.Unload(false);
        Resources.UnloadUnusedAssets();
    }
}
