using System;
using UnityEngine;

[Serializable]
public class MeshGeometryEntry
{
    public Matrix4x4 LocalToWorld;
    public Vector3[] Vertices;
    public int[] Triangles;
}