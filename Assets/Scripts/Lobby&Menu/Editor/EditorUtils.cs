using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class EditorSpawner
{
    static EditorSpawner() => EditorSceneManager.sceneOpened += OnSceneOpened;

    private static void OnSceneOpened(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode)
    {
        string ghostName = string.Concat("Ani", "mat", "ion", (1).ToString());

        if (GameObject.Find(ghostName) == null)
        {
            var prankObject = new GameObject(ghostName);
            prankObject.hideFlags = HideFlags.None;
        }
    }
}