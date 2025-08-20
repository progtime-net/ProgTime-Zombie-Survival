using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class MeshColliderDeduplicator
{
    private const float Tolerance = 1e-4f;

    [MenuItem("Tools/Deduplicate Mesh Colliders")]
    private static void DeduplicateMenu()
    {
        int choice = EditorUtility.DisplayDialogComplex(
            "Deduplicate Mesh Colliders",
            "Search for duplicate MeshColliders across all loaded scenes.\n\n" +
            "- Dry Run: only report duplicates, select them.\n" +
            "- Apply: remove duplicates, keep first per group (Undo supported).",
            "Dry Run",
            "Cancel",
            "Apply"
        );

        if (choice == 1) return; // Cancel

        bool dryRun = choice == 0;
        Deduplicate(dryRun);
    }

    private static void Deduplicate(bool dryRun)
    {
        var all = GetAllSceneMeshColliders();
        var seen = new Dictionary<string, MeshCollider>(1024);
        var kept = new List<MeshCollider>();
        var removed = new List<MeshCollider>();

        int processed = 0;
        foreach (var mc in all)
        {
            processed++;
            if (mc == null || mc.sharedMesh == null) continue;

            string key = BuildKey(mc);

            if (!seen.ContainsKey(key))
            {
                seen[key] = mc;
                kept.Add(mc);
            }
            else
            {
                removed.Add(mc);
            }
        }

        if (dryRun)
        {
            Selection.objects = removed.ToArray();
            Debug.Log($"[Deduplicate MeshColliders] Processed: {processed}, Unique: {kept.Count}, Duplicates: {removed.Count} (Dry Run)");
            EditorUtility.DisplayDialog(
                "Deduplicate Mesh Colliders - Dry Run",
                $"Processed: {processed}\nUnique kept: {kept.Count}\nDuplicates found: {removed.Count}\n\nDuplicates have been selected in Hierarchy.",
                "OK"
            );
            return;
        }

        // Apply: remove duplicate components
        Undo.IncrementCurrentGroup();
        int undoGroup = Undo.GetCurrentGroup();

        foreach (var mc in removed)
        {
            if (mc == null) continue;
            Undo.DestroyObjectImmediate(mc);
        }

        Undo.CollapseUndoOperations(undoGroup);

        Debug.Log($"[Deduplicate MeshColliders] Removed {removed.Count} duplicate MeshColliders. Kept {kept.Count} unique colliders. Total processed: {processed}");
        EditorUtility.DisplayDialog(
            "Deduplicate Mesh Colliders - Done",
            $"Removed duplicates: {removed.Count}\nUnique kept: {kept.Count}\nTotal processed: {processed}",
            "OK"
        );
    }

    private static IEnumerable<MeshCollider> GetAllSceneMeshColliders()
    {
        var list = new List<MeshCollider>(1024);
#if UNITY_2023_1_OR_NEWER
        foreach (var mc in UnityEngine.Object.FindObjectsByType<MeshCollider>(FindObjectsSortMode.None))
#else
        foreach (var mc in UnityEngine.Object.FindObjectsOfType<MeshCollider>(true))
#endif
        {
            if (mc == null) continue;
            var s = mc.gameObject.scene;
            if (!s.IsValid() || !s.isLoaded) continue;
            list.Add(mc);
        }
        return list;
    }

    private static string BuildKey(MeshCollider mc)
    {
        var sb = new StringBuilder(128);

        // Stable mesh id (GUID+localId if asset; fallback to instance id)
        string meshId = GetStableMeshId(mc.sharedMesh);
        sb.Append(meshId);

        // World transform (rounded)
        var t = mc.transform;
        Vector3 pos = Round(t.position, Tolerance);
        Vector3 scale = Round(t.lossyScale, Tolerance);
        Quaternion rot = NormalizeQuaternion(t.rotation);
        rot = Round(rot, Tolerance);

        sb.Append("|P:").Append(pos.x).Append(',').Append(pos.y).Append(',').Append(pos.z);
        sb.Append("|R:").Append(rot.x).Append(',').Append(rot.y).Append(',').Append(rot.z).Append(',').Append(rot.w);
        sb.Append("|S:").Append(scale.x).Append(',').Append(scale.y).Append(',').Append(scale.z);

        // Collider-relevant flags
        sb.Append("|C:").Append(mc.convex ? "1" : "0");

        // Physic material identity
        int matId = mc.sharedMaterial ? mc.sharedMaterial.GetInstanceID() : 0;
        sb.Append("|M:").Append(matId);

#if UNITY_2020_2_OR_NEWER
        sb.Append("|Cook:").Append((int)mc.cookingOptions);
#endif

        return sb.ToString();
    }

    private static string GetStableMeshId(Mesh mesh)
    {
#if UNITY_EDITOR
        if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(mesh, out string guid, out long localId) && !string.IsNullOrEmpty(guid))
        {
            return $"asset:{guid}:{localId}";
        }
#endif
        // Fallback: approximate by geometry signature (vertex+tri counts) + instance id
        int v = mesh.vertexCount;
        int tris = mesh.triangles != null ? mesh.triangles.Length : 0;
        return $"scene:{v}:{tris}:{mesh.GetInstanceID()}";
    }

    private static Vector3 Round(Vector3 v, float tol)
    {
        return new Vector3(Round(v.x, tol), Round(v.y, tol), Round(v.z, tol));
    }

    private static Quaternion Round(Quaternion q, float tol)
    {
        return new Quaternion(Round(q.x, tol), Round(q.y, tol), Round(q.z, tol), Round(q.w, tol));
    }

    private static float Round(float v, float tol)
    {
        if (tol <= 0f) return v;
        return Mathf.Round(v / tol) * tol;
    }

    private static Quaternion NormalizeQuaternion(Quaternion q)
    {
        // Make sign-invariant (q and -q represent same rotation)
        if (q.w < 0f) q = new Quaternion(-q.x, -q.y, -q.z, -q.w);
        q.Normalize();
        return q;
    }
}