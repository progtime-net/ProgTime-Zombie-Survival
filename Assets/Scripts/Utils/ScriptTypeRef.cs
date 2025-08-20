using UnityEditor;
using UnityEngine;

/// <summary>
/// Весь класс чтоб можно было выбрать тип в инспекторе Unity.
/// </summary>
[System.Serializable]
public class ScriptTypeRef
{
    public MonoScript script;                 
    public System.Type Type => script ? script.GetClass() : null;
}
