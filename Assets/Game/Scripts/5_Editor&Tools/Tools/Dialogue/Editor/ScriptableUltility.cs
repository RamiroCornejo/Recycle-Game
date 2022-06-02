using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class ScriptableUltility
{
    public static T CreateScriptable<T>(string path, string name) where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        if(!Directory.Exists(Application.dataPath + path))
        {
            Directory.CreateDirectory(Application.dataPath + path);
        }

        string finalPath = AssetDatabase.GenerateUniqueAssetPath("Assets" + path + name + ".asset");
        AssetDatabase.CreateAsset(asset, finalPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();

        //Selection.activeObject = asset;

        return asset;
    }
    public static T CreateScriptable<T>(string completePath) where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        string finalPath = AssetDatabase.GenerateUniqueAssetPath(completePath);
        AssetDatabase.CreateAsset(asset, finalPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();

        //Selection.activeObject = asset;

        return asset;
    }
}
