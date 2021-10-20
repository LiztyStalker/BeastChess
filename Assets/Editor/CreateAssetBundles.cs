using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    public static void BuildAllAssetBundles()
    {   
        string directory = UnityEngine.Application.streamingAssetsPath;
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}
