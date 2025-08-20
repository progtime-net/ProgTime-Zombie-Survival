using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Reference to a script type for use in Unity.
/// </summary>
[System.Serializable]
public class ScriptTypeRef
{
#if UNITY_EDITOR
    public MonoScript script;
    public System.Type Type => script ? script.GetClass() : null;
#else
    // Provide alternative fields or leave empty for builds
#endif
}