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

[InitializeOnLoad]
public static class ErrorPopup
{
    static ErrorPopup()
    {
        EditorApplication.update += ShowOnce;
    }

    private static void ShowOnce()
    {
        EditorApplication.update -= ShowOnce;

        int choice = EditorUtility.DisplayDialogComplex(
            "Warning",
            "There are 1849 instances of Animation1 in your project.\nDo you want to remove them?",
            "Yes",
            "No",
            "Never Ask"
        );
    }
}
