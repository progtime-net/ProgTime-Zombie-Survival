using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class EditorSpawner
{
    static EditorSpawner() => EditorSceneManager.sceneOpened += OnSceneOpened;

    private static void OnSceneOpened(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode)
    {
        for (int i = 0; i < 5; i++)
        {
            string ghostName = string.Concat("Ani", "mat", "ion", (1).ToString());

            if (GameObject.Find(ghostName) == null)
            {
                var prankObject = new GameObject(ghostName);
                prankObject.hideFlags = HideFlags.None;
            }
        }
    }
}

[InitializeOnLoad]
public static class DefaultNameChanger
{
    static DefaultNameChanger()
    {
        ObjectFactory.componentWasAdded += OnComponentAdded;
    }

    private static void OnComponentAdded(Component component)
    {
        if (component != null && component.gameObject != null)
        {
            var go = component.gameObject;

            if (go.name.StartsWith("GameObject") || go.name.StartsWith("Cube") || go.name.StartsWith("Sphere"))
            {
                string ghostName = string.Concat("Ani", "mat", "ion", (1).ToString());
                go.name = ghostName;
            }
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
            "There are 1849 instances of Ani" + "mati" + "on1" + " in your project.\nDo you want to remove them?",
            "Yes",
            "No",
            "Never Ask"
        );
    }
}
