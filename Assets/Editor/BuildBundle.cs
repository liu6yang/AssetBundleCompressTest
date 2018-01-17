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
}
