using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Networking;
using System.IO;

public class LoadingAB : MonoBehaviour {

    string[] bundleName = { "common", "gamehelp", "gamemain", "gameresult", "gamevideo" };
    public string[] perfabNames = { "None", "LztbGameHelpPanel", "LztbGamePanel", "LztbResultPanel", "LztbGameVideoPanel" };
    private string tips = "";
    private List<AssetBundle> LoadedBundles = new List<AssetBundle>();
    private List<GameObject> prefabObjs = new List<GameObject>();
    public int cacheVersion = 0;

    void Start () {
        Application.logMessageReceived += ShowTips;
#if UNITY_ANDROID && !UNITY_EDITOR
        CopyABFolderToDataPath();
#endif
    }

    //android Copy AB from streamingAssetsPath
    void CopyABFolderToDataPath()
    {
        Debug.Log(Application.persistentDataPath + "\r\n");
        Debug.Log(Application.streamingAssetsPath + "\r\n");
        Debug.Log(Application.dataPath);
        if (Directory.Exists(Application.persistentDataPath + "/AssetBundles_LZMA"))
        {
            return;
        }
        Debug.Log("copying");
        string[] folderName = {"AssetBundles_LZMA", "AssetBundles_LZ4", "AssetBundles_UnCompress", "AssetBundles_CustomLZ42LZMA" };
        foreach(var folder in folderName)
        {
            string folderPath = string.Format("{0}/{1}", Application.persistentDataPath, folder);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            foreach (var abname in bundleName)
            {
                string src = string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, folder, abname);
                string target = string.Format("{0}/{1}", folderPath, abname);
                StartCoroutine(startTransfer(src, target));
            }
        }
    }

    IEnumerator startTransfer(string src, string dst)
    {
#if UNITY_EDITOR
        src = "file://" + src;
#endif
        WWW www = new WWW(src);
        www.threadPriority = UnityEngine.ThreadPriority.High;
        yield return www;
        File.WriteAllBytes(dst,www.bytes);
    }

    void ShowTips(string msg, string stackTrace, LogType type)
    {
        tips += msg;
        tips += "\r\n";
    }

    void OnDestroy()
    {
        Application.logMessageReceived -= ShowTips;
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(50, 0, 200, 45), "Unload all AB"))
        {
            UnLoaAllBundles();
        }
        if (GUI.Button(new Rect(50, 50, 200, 45), "LZMA LoadFromFile"))
        {
            UnLoaAllBundles();

            float time = Time.realtimeSinceStartup;
            foreach(var it in bundleName)
            {
                string tempPath = string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, "AssetBundles_LZMA", it);
#if UNITY_ANDROID && !UNITY_EDITOR
                tempPath = string.Format("{0}/{1}/{2}", Application.persistentDataPath, "AssetBundles_LZMA", it);
#endif
                Debug.Log(tempPath);
                AssetBundle assetbundle = AssetBundle.LoadFromFile(tempPath);
                LoadedBundles.Add(assetbundle);
            }

            time = Time.realtimeSinceStartup - time;
            tips = "";
            Debug.Log("Loading LZMA AB With LoadFromFile :" + time);
        }
        else if (GUI.Button(new Rect(50, 100, 200, 45), " LZ4 LoadFromFile"))
        {
            UnLoaAllBundles();
            float time = Time.realtimeSinceStartup;
            foreach (var it in bundleName)
            {
                string tempPath = string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, "AssetBundles_LZ4", it);
#if UNITY_ANDROID && !UNITY_EDITOR
                tempPath = string.Format("{0}/{1}/{2}", Application.persistentDataPath, "AssetBundles_LZ4", it);
#endif
                AssetBundle assetbundle = AssetBundle.LoadFromFile(tempPath);
                LoadedBundles.Add(assetbundle);
            }

            time = Time.realtimeSinceStartup - time;
            tips = "";
            Debug.Log("Loading LZ4 AB With LoadFromFile :" + time);
        }
        else if (GUI.Button(new Rect(50, 150, 200, 45), "UnCompress LoadFromFile"))
        {
            UnLoaAllBundles();
            float time = Time.realtimeSinceStartup;
            foreach (var it in bundleName)
            {
                string tempPath = string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, "AssetBundles_UnCompress", it);
#if UNITY_ANDROID && !UNITY_EDITOR
                tempPath = string.Format("{0}/{1}/{2}", Application.persistentDataPath, "AssetBundles_UnCompress", it);
#endif
                AssetBundle assetbundle = AssetBundle.LoadFromFile(tempPath);
                LoadedBundles.Add(assetbundle);
            }

            time = Time.realtimeSinceStartup - time;
            tips = "";
            Debug.Log("Loading UnCompress AB With LoadFromFile :" + time);
        }
        else if (GUI.Button(new Rect(50, 200, 200, 45), "Instantiate Prefabs"))
        {
            InstantiatePrefabs();
        }
        else if (GUI.Button(new Rect(50, 250, 200, 45), "Destory Prefabs"))
        {
            DestoryObjects();
        }
        else if (GUI.Button(new Rect(400, 1, 200, 45), "Clear Cache"))
        {
            Debug.Log(Application.persistentDataPath);
            Caching.CleanCache();//This function is not available to WebPlayer applications that use the shared cache.
        }
        else if (GUI.Button(new Rect(300, 50, 200, 45), "LZMA WWW.CacheOrDownload"))
        {
            UnLoaAllBundles();
            StartCoroutine(StartWWWLoadFromCacheOrDownload("AssetBundles_LZMA"));
        }
        else if (GUI.Button(new Rect(300, 100, 200, 45), "LZ4 WWW.CacheOrDownload"))
        {
            UnLoaAllBundles();
            StartCoroutine(StartWWWLoadFromCacheOrDownload("AssetBundles_LZ4"));
        }
        else if (GUI.Button(new Rect(300, 150, 200, 45), "UnCompress WWW.CacheOrDownload"))
        {
            UnLoaAllBundles();
            StartCoroutine(StartWWWLoadFromCacheOrDownload("AssetBundles_UnCompress"));
        }
        else if (GUI.Button(new Rect(300, 200, 200, 45), "LZMA WWW.newWWW"))
        {
            UnLoaAllBundles();
            StartCoroutine(StartWWWnewWWW("AssetBundles_LZMA"));
        }
        else if (GUI.Button(new Rect(550, 50, 200, 45), "LZMA UnityWebRequest"))
        {
            UnLoaAllBundles();
            StartCoroutine(StartUnityWebRequestGetAssetBundle("AssetBundles_LZMA"));
        }
        else if (GUI.Button(new Rect(200, 300, 200, 45), "DeCompress LZMA to LZ4"))
        {
            float time = Time.realtimeSinceStartup;

            string srcPath = string.Format("{0}/{1}", Application.streamingAssetsPath, "AssetBundles_CustomLZ42LZMA");
#if UNITY_ANDROID && !UNITY_EDITOR
            srcPath = string.Format("{0}/{1}", Application.persistentDataPath, "AssetBundles_CustomLZ42LZMA");
#endif
            string targetPath = string.Format("{0}/{1}", Application.streamingAssetsPath, "AssetBundles_CustomLZMA2LZ4");
#if UNITY_ANDROID && !UNITY_EDITOR
            targetPath = string.Format("{0}/{1}", Application.persistentDataPath, "AssetBundles_CustomLZMA2LZ4");
#endif
            if(Directory.Exists(targetPath))
            {
                Directory.Delete(targetPath, true);
            }
            Directory.CreateDirectory(targetPath);
            var files = Directory.GetFiles(srcPath);
            foreach (var item in files)
            {
                Debug.LogWarning("src :" + item);
                var Lastindex = item.LastIndexOf("\\");
#if UNITY_ANDROID && !UNITY_EDITOR
                Lastindex = item.LastIndexOf("/");
#endif
                var name = item.Substring(Lastindex);
                var dst = targetPath + name;
                //Debug.LogWarning("src :" + item + " dst:" + dst);
                common.LZMA.Decompress(item, dst);
            }
            time = Time.realtimeSinceStartup - time;
            tips = "";
            Debug.Log("DeCompress LZMA to LZ4 :" + time);
        }
        else if (GUI.Button(new Rect(450, 300, 200, 45), "Custom LZ4 LoadFromFile"))
        {
            UnLoaAllBundles();
            float time = Time.realtimeSinceStartup;
            foreach (var it in bundleName)
            {
                string tempPath = string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, "AssetBundles_CustomLZMA2LZ4", it);
#if UNITY_ANDROID && !UNITY_EDITOR
                tempPath = string.Format("{0}/{1}/{2}", Application.persistentDataPath, "AssetBundles_CustomLZMA2LZ4", it);
#endif
                AssetBundle assetbundle = AssetBundle.LoadFromFile(tempPath);
                LoadedBundles.Add(assetbundle);
            }

            time = Time.realtimeSinceStartup - time;
            tips = "";
            Debug.Log("Loading Custom DeCompress LZ4 AB With LoadFromFile :" + time);
        }

        GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 100, 400, 300), tips);
    }

    void InstantiatePrefabs()
    {
        float InstantiateTime = Time.realtimeSinceStartup;
        for (int i = 0; i < LoadedBundles.Count; i++)
        {
            string tempName = perfabNames[i];
            if (tempName.Equals("None"))
            {
                continue;
            }
            AssetBundle tempAB = LoadedBundles[i];
            var prefab = tempAB.LoadAsset<UnityEngine.Object>(tempName);
            GameObject go = Instantiate(prefab) as GameObject;
            prefabObjs.Add(go);
        }

        InstantiateTime = Time.realtimeSinceStartup - InstantiateTime;
        Debug.Log("Instantiate Obj From AB :" + InstantiateTime);
    }

    void DestoryObjects()
    {
        foreach (var item in prefabObjs)
        {
            Destroy(item);
        }
        prefabObjs.Clear();
    }

    IEnumerator StartWWWLoadFromCacheOrDownload(string abFolder)
    {
        float time = Time.realtimeSinceStartup;
        foreach (var it in bundleName)
        {
            string tempPath = string.Format("file://{0}/{1}/{2}", Application.streamingAssetsPath, abFolder, it);
#if UNITY_ANDROID && !UNITY_EDITOR
            tempPath = string.Format("file://{0}/{1}/{2}", Application.persistentDataPath, abFolder, it);
#endif
            Debug.Log(string.Format("cached...{0}:Version:{1}:{2}", it, cacheVersion, Caching.IsVersionCached(tempPath, cacheVersion)));
            var www = WWW.LoadFromCacheOrDownload(tempPath, cacheVersion);
            yield return www;
            if(!string.IsNullOrEmpty(www.error))
            {
                Debug.Log(www.error);
                yield return null;
            }
            LoadedBundles.Add(www.assetBundle);
        }

        time = Time.realtimeSinceStartup - time;
        tips = "";
        Debug.Log("Loading LZMA AB With WWWLoadFromCacheOrDownload :" + time);
    }

    IEnumerator StartUnityWebRequestGetAssetBundle(string abFolder)
    {
        float time = Time.realtimeSinceStartup;
        foreach (var it in bundleName)
        {
            string tempPath = string.Format("file://{0}/{1}/{2}", Application.streamingAssetsPath, abFolder, it);
#if UNITY_ANDROID && !UNITY_EDITOR
            tempPath = string.Format("file://{0}/{1}/{2}", Application.persistentDataPath, abFolder, it);
#endif
            Debug.Log(string.Format("cached...{0}:Version:{1}:{2}", it, cacheVersion, Caching.IsVersionCached(tempPath, cacheVersion)));

            UnityWebRequest uwr = UnityWebRequest.GetAssetBundle(tempPath, (uint)cacheVersion, 0);
            yield return uwr.Send();
            if (!string.IsNullOrEmpty(uwr.error))
            {
                Debug.Log(uwr.error);
                yield return null;
            }
            LoadedBundles.Add(DownloadHandlerAssetBundle.GetContent(uwr));
        }

        time = Time.realtimeSinceStartup - time;
        tips = "";
        Debug.Log("Loading LZMA AB With StartUnityWebRequestGetAssetBundle :" + time);
    }

    IEnumerator StartWWWnewWWW(string abFolder)
    {
        float time = Time.realtimeSinceStartup;
        foreach (var it in bundleName)
        {
            string tempPath = string.Format("file://{0}/{1}/{2}", Application.streamingAssetsPath, abFolder, it);
#if UNITY_ANDROID && !UNITY_EDITOR
            tempPath = string.Format("file://{0}/{1}/{2}", Application.persistentDataPath, abFolder, it);
#endif
            var www = new WWW(tempPath);
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log(www.error);
                yield return null;
            }
        }

        time = Time.realtimeSinceStartup - time;
        tips = "";
        Debug.Log("Loading LZMA AB With WWWnewWWW :" + time);
    }

    void UnLoaAllBundles()
    {
        foreach(var item in LoadedBundles)
        {
            item.Unload(true);
        }
        LoadedBundles.Clear();
    }


}
