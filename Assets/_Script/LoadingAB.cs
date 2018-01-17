using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadingAB : MonoBehaviour {

    string[] bundleName = { "common", "gamehelp", "gamemain", "gameresult", "gamevideo" };
    private string tips = "";
    private List<AssetBundle> LoadedBundles = new List<AssetBundle>();


    void Start () {
        Application.logMessageReceived += ShowTips;


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
        if (GUI.Button(new Rect(50, 50, 200, 45), "LZMA LoadFromFile"))
        {
            UnLoaAllBundles();

            float time = Time.realtimeSinceStartup;
            foreach(var it in bundleName)
            {
                string tempPath = string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, "AssetBundles_LZMA", it);
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
                Debug.Log(tempPath);
                AssetBundle assetbundle = AssetBundle.LoadFromFile(tempPath);
                LoadedBundles.Add(assetbundle);
            }

            time = Time.realtimeSinceStartup - time;
            tips = "";
            Debug.Log("Loading LZMA AB With LoadFromFile :" + time);
        }
        else if (GUI.Button(new Rect(50, 150, 200, 45), "UnCompress LoadFromFile"))
        {
            UnLoaAllBundles();
            float time = Time.realtimeSinceStartup;
            foreach (var it in bundleName)
            {
                string tempPath = string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, "AssetBundles_UnCompress", it);
                AssetBundle assetbundle = AssetBundle.LoadFromFile(tempPath);
                LoadedBundles.Add(assetbundle);
            }

            time = Time.realtimeSinceStartup - time;
            tips = "";
            Debug.Log("Loading LZMA AB With LoadFromFile :" + time);
        }
        else if (GUI.Button(new Rect(300, 50, 200, 45), "LZMA  WWW"))
        {

        }

        //instanite ??
        
        GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 100, 400, 300), tips);
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
