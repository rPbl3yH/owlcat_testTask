using UnityEngine;

public static class MeshIntersection
{
    public static bool RaycastDown(MeshGeometryEntry entry, Vector3 worldOrigin, out float hitY)
    {
        hitY = float.NaN;

        Matrix4x4 inverseMatrix = entry.LocalToWorld.inverse;
        Vector3 localOrigin = inverseMatrix.MultiplyPoint(worldOrigin);
        Vector3 normalized = inverseMatrix.MultiplyVector(Vector3.down).normalized;

        Vector3[] verts = entry.Vertices;
        int[] tris = entry.Triangles;

        bool found = false;
        float bestT = float.MaxValue;

        for (int i = 0; i < tris.Length; i += 3)
        {
            Vector3 v0 = verts[tris[i]];
            Vector3 v1 = verts[tris[i + 1]];
            Vector3 v2 = verts[tris[i + 2]];

            if (IntersectRayTriangle(localOrigin, normalized, v0, v1, v2, out float t, out _))
            {
                if (t >= 0f && t < bestT)
                {
                    bestT = t;
                    found = true;
                }
            }
        }

        if (found)
        {
            Vector3 hitLocal = localOrigin + normalized * bestT;
            Vector3 hitWorld = entry.LocalToWorld.MultiplyPoint(hitLocal);
            hitY = hitWorld.y;
            return true;
        }

        return false;
    }

    private static bool IntersectRayTriangle(Vector3 origin, Vector3 dir,
        Vector3 v0, Vector3 v1, Vector3 v2,
        out float t, out Vector3 bary)
    {
        bary = Vector3.zero;
        t = 0f;

        Vector3 edge1 = v1 - v0;
        Vector3 edge2 = v2 - v0;
        Vector3 pvec = Vector3.Cross(dir, edge2);
        float det = Vector3.Dot(edge1, pvec);
        if (Mathf.Abs(det) < Constants.EPSILON) return false;
        float invDet = 1f / det;

        Vector3 tvec = origin - v0;
        float u = Vector3.Dot(tvec, pvec) * invDet;
        if (u < 0f || u > 1f) return false;

        Vector3 qvec = Vector3.Cross(tvec, edge1);
        float v = Vector3.Dot(dir, qvec) * invDet;
        if (v < 0f || u + v > 1f) return false;

        t = Vector3.Dot(edge2, qvec) * invDet;
        bary = new Vector3(1 - u - v, u, v);
        return true;
    }
}