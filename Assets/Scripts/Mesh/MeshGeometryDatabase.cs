using UnityEngine;

[CreateAssetMenu(fileName = "MeshGeometryDatabase", menuName = "Geometry/MeshGeometryDatabase")]
public class MeshGeometryDatabase : ScriptableObject
{
    public MeshGeometryEntry[] Entries;
}