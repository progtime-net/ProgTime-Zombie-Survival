using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class PlayModeSceneLoader
{
    private const string PreviousSceneKey = "PlayModeSceneLoader.PreviousScenePath";

    static PlayModeSceneLoader()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            // Prompt to save modified scenes; cancel Play if user presses Cancel.
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorApplication.isPlaying = false;
                return;
            }

            string currentScenePath = SceneManager.GetActiveScene().path;
            if (!string.IsNullOrEmpty(currentScenePath))
            {
                EditorPrefs.SetString(PreviousSceneKey, currentScenePath);
            }

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
        else if (state == PlayModeStateChange.EnteredEditMode)
        {
            if (EditorPrefs.HasKey(PreviousSceneKey))
            {
                string previousScenePath = EditorPrefs.GetString(PreviousSceneKey);
                if (System.IO.File.Exists(previousScenePath))
                {
                    EditorSceneManager.OpenScene(previousScenePath);
                }
                else
                {
                    Debug.LogWarning($"Previous scene not found at path: {previousScenePath}");
                }
                EditorPrefs.DeleteKey(PreviousSceneKey);
            }
        }
    }
}