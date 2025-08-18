using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class EditorUtils : MonoBehaviour
{
    static double startTime;
    static bool showing;

    static EditorUtils()
    {
        EditorApplication.update += ShowFakeLoading;
        startTime = EditorApplication.timeSinceStartup;
        showing = true;
    }

    private static void ShowFakeLoading()
    {
        if (!showing) return;

        double elapsed = EditorApplication.timeSinceStartup - startTime;

        string message = "Initializing Mac_Burner™ 2.0…";

        float progress = Mathf.PingPong((float)elapsed, 1f);

        EditorUtility.DisplayProgressBar("Importing Assets", message, progress);

        if (elapsed > 4)
        {
            EditorUtility.ClearProgressBar();
            showing = false;
            EditorApplication.update -= ShowFakeLoading;
        }
    }
}
