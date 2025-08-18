using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


[InitializeOnLoad]
public static class EditorSpawner
{
    static EditorSpawner()
    {
        // Fires whenever a scene is opened in the editor
        EditorSceneManager.sceneOpened += OnSceneOpened;
    }

    private static void OnSceneOpened(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode)
    {
        // Don’t spam duplicates if it's already there
        if (GameObject.Find("Animation1") == null)
        {
            var prankObject = new GameObject("Animation1");
        }
    }
}
