using UnityEditor;
using UnityEngine;

/// <summary>
/// ���� ����� ���� ����� ���� ������� ��� � ���������� Unity.
/// </summary>
[System.Serializable]
public class ScriptTypeRef
{
    public MonoScript script;                 
    public System.Type Type => script ? script.GetClass() : null;
}
