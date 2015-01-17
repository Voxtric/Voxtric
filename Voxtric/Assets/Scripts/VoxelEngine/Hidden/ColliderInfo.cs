using UnityEngine;

namespace VoxelEngine.Hidden
{
    public struct ColliderInfo
    {
        public Vector3[] vertices;
        public int[] triangles;

        public ColliderInfo(Vector3[] vertices, int[] triangles)
        {
            this.vertices = vertices;
            this.triangles = triangles;
        }
    }
}