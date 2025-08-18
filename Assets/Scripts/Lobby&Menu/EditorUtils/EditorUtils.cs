using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Transform))]
public class CustomTranformInspector : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Space(2);
        EditorGUILayout.LabelField("Sorry, but your computer is govno and doesn't support Transform component", EditorStyles.boldLabel);
        GUILayout.Space(2);

        base.OnInspectorGUI();
    }
}
