using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public static class OpenFolder
{
    [MenuItem("EditorTools/Open Folder/Data Path", false, 10)]
    public static void OpenFolderDataPath()
    {
        Execute(Application.dataPath);
    }

    [MenuItem("EditorTools/Open Folder/Persistent Data Path", false, 11)]
    public static void OpenFolderPersistentDataPath()
    {
        Execute(Application.persistentDataPath);
    }

    [MenuItem("EditorTools/Open Folder/Streaming Data Path", false, 12)]
    public static void OpenFolderStreamingDataPath()
    {
        Execute(Application.streamingAssetsPath);
    }

    [MenuItem("EditorTools/Open Folder/Temporary Cache Path", false, 13)]
    public static void OpenFolderTemporaryCachePath()
    {
        Execute(Application.temporaryCachePath);
    }

    private static void Execute(string path)
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsEditor:
                Process.Start("Explorer.exe", path.Replace('/', '\\'));
                break;
            case RuntimePlatform.OSXEditor:
                Process.Start("open", path);
                break;
        }
    }
}
