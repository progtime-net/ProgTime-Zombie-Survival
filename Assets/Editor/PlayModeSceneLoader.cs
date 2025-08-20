using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class PlayModeSceneLoader
{
    static PlayModeSceneLoader()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            string sceneToLoadPath = "Assets/Scenes/MainMenuScene.unity"; 

            if (System.IO.File.Exists(sceneToLoadPath))
            {
                EditorSceneManager.OpenScene(sceneToLoadPath);
            }
            else
            {
                Debug.LogWarning($"Scene not found at path: {sceneToLoadPath}");
            }
        }
    }
}