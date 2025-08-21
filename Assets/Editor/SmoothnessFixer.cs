// csharp
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class SetSmoothnessForSelection
{
    public const float DefaultSmoothness = 0.16f;

    [MenuItem("Tools/Materials/Set Smoothness...")]
    private static void OpenWindow()
    {
        SmoothnessSetterWindow.Open(DefaultSmoothness);
    }

    [MenuItem("Tools/Materials/Set Smoothness...", true)]
    private static bool ValidateOpenWindow()
    {
        return Selection.objects != null && Selection.objects.Length > 0;
    }

    [MenuItem("Tools/Materials/Set Smoothness to 0.16")]
    private static void SetToDefault()
    {
        ApplySmoothness(DefaultSmoothness);
    }

    [MenuItem("Tools/Materials/Set Smoothness to 0.16", true)]
    private static bool ValidateSetToDefault()
    {
        return Selection.objects != null && Selection.objects.Length > 0;
    }

    public static void ApplySmoothness(float value)
    {
        var materials = CollectSelectedMaterials();
        if (materials.Count == 0)
        {
            EditorUtility.DisplayDialog("Set Smoothness", "No materials found in selection.", "OK");
            return;
        }

        float clamped = Mathf.Clamp01(value);

        Undo.IncrementCurrentGroup();
        int undoGroup = Undo.GetCurrentGroup();
        int changedCount = 0;

        foreach (var mat in materials)
        {
            if (mat == null) continue;

            bool changed = false;
            Undo.RecordObject(mat, $"Set Smoothness to {clamped:0.###}");

            if (mat.HasProperty("_Smoothness"))
            {
                mat.SetFloat("_Smoothness", clamped);
                changed = true;
            }
            if (mat.HasProperty("_Glossiness"))
            {
                mat.SetFloat("_Glossiness", clamped);
                changed = true;
            }
            if (mat.HasProperty("_GlossMapScale"))
            {
                mat.SetFloat("_GlossMapScale", clamped);
                changed = true;
            }
            if (mat.HasProperty("_PerceptualSmoothness")) // HDRP
            {
                mat.SetFloat("_PerceptualSmoothness", clamped);
                changed = true;
            }

            if (changed)
            {
                EditorUtility.SetDirty(mat);
                changedCount++;
            }
        }

        Undo.CollapseUndoOperations(undoGroup);

        Debug.Log($"[SetSmoothness] Processed {materials.Count} materials. Changed: {changedCount}. Target: {clamped:0.###}");
        EditorUtility.DisplayDialog("Set Smoothness",
            $"Materials processed: {materials.Count}\nMaterials changed: {changedCount}\nSmoothness set to: {clamped:0.###}",
            "OK");
    }

    internal static HashSet<Material> CollectSelectedMaterials()
    {
        var set = new HashSet<Material>();

        foreach (var obj in Selection.objects)
        {
            if (obj is Material matAsset)
            {
                set.Add(matAsset);
                continue;
            }

            if (obj is GameObject go)
            {
                var renderers = go.GetComponentsInChildren<Renderer>(true);
                foreach (var r in renderers)
                {
                    if (r == null) continue;
                    var shared = r.sharedMaterials;
                    for (int i = 0; i < shared.Length; i++)
                    {
                        var m = shared[i];
                        if (m != null) set.Add(m);
                    }
                }
            }
        }

        return set;
    }
}


public sealed class SmoothnessSetterWindow : EditorWindow
{
    private float _value = SetSmoothnessForSelection.DefaultSmoothness;
    private int _materialCount;

    public static void Open(float defaultValue)
    {
        var win = CreateInstance<SmoothnessSetterWindow>();
        win.titleContent = new GUIContent("Set Smoothness");
        win._value = defaultValue;
        win.minSize = new Vector2(320, 120);
        win.RefreshSelectionCount();
        win.ShowUtility();
    }

    private void OnFocus() => RefreshSelectionCount();
    private void OnHierarchyChange() => RefreshSelectionCount();
    private void OnProjectChange() => RefreshSelectionCount();

    private void RefreshSelectionCount()
    {
        _materialCount = SetSmoothnessForSelection.CollectSelectedMaterials().Count;
        Repaint();
    }

    private void OnGUI()
    {
        GUILayout.Label("Apply smoothness to materials in selection", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Materials found", _materialCount.ToString());
        _value = EditorGUILayout.Slider("Smoothness (0..1)", _value, 0f, 1f);

        EditorGUILayout.Space();

        using (new EditorGUI.DisabledScope(_materialCount == 0))
        {
            if (GUILayout.Button($"Apply ({_value:0.###})"))
            {
                SetSmoothnessForSelection.ApplySmoothness(_value);
                Close();
            }
        }

        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
    }
}