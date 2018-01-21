using UnityEditor;
using UnityEngine;
using System.IO;

public class BuildBundle {

    [MenuItem("LiuYangTest/AB LZMA")]
    static void BuildAB_LZMA()
    {
        string assetBundleDirectory = Application.streamingAssetsPath + "/AssetBundles_LZMA";
        if (!Directory.Exists(assetBundleDirectory))
        { 
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.Android);
    }

    [MenuItem("LiuYangTest/AB LZ4")]
    static void BuildAB_LZ4()
    {
        string assetBundleDirectory = Application.streamingAssetsPath + "/AssetBundles_LZ4";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
    }

    [MenuItem("LiuYangTest/AB UnCompress")]
    static void BuildAB_UnCompress()
    {
        string assetBundleDirectory = Application.streamingAssetsPath + "/AssetBundles_UnCompress";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.Android);
    }

    [MenuItem("LiuYangTest/AB LZ4 custom to LZMA")]
    static void CustomCompressLZMA2LZ4()
    {
        string lz4Path = Application.streamingAssetsPath + "/AssetBundles_LZ4";
        if (!Directory.Exists(lz4Path))
        {
            Debug.LogError("not exits AssetBundles_LZ4");
            return;
        }
        string targetPath = Application.streamingAssetsPath + "/AssetBundles_CustomLZ42LZMA";
        if (!Directory.Exists(targetPath))
        {
            Directory.CreateDirectory(targetPath);
        }
        var files = Directory.GetFiles(lz4Path);
        foreach(var item in files)
        {
            var Lastindex = item.LastIndexOf("\\");
            var name = item.Substring(Lastindex);
            var dst = targetPath + name;
            Debug.LogWarning("src :" + item + " dst:" + dst);
            common.LZMA.Compress(item , dst);
        }
    }
}
