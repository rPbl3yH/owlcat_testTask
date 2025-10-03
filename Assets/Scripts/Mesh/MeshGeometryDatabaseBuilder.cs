#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public static class MeshGeometryDatabaseBuilder
{
    [MenuItem("Tools/Build Mesh Geometry DB")]
    public static void Build()
    {
        List<MeshGeometryEntry> entries = new List<MeshGeometryEntry>();

        MeshFilter[] meshFilters = Object.FindObjectsOfType<MeshFilter>();
        foreach (MeshFilter mf in meshFilters)
        {
            if (mf.sharedMesh == null)
                continue;

            MeshGeometryEntry entry = new MeshGeometryEntry
            {
                LocalToWorld = mf.transform.localToWorldMatrix,
                Vertices = mf.sharedMesh.vertices,
                Triangles = mf.sharedMesh.triangles
            };
            entries.Add(entry);
        }

        MeshGeometryDatabase db = ScriptableObject.CreateInstance<MeshGeometryDatabase>();
        db.Entries = entries.ToArray();

        string path = EditorUtility.SaveFilePanelInProject("Save Mesh Geometry DB", "MeshGeometryDB", "asset", "Choose path");
        if (string.IsNullOrEmpty(path) == false)
        {
            AssetDatabase.CreateAsset(db, path);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = db;
        }
    }
}
#endif