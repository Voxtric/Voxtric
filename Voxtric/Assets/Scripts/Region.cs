using UnityEngine;
using System.Collections.Generic;

namespace VoxelEngine
{
    public sealed class Region : MonoBehaviour
    {
        private VoxelData _voxelData;

        private Vector3[] _vertices;
        private int[] _triangles;
        private Vector2[] _uv;
        private int _faceCount;

        private Mesh _mesh;
        private MeshCollider _collider;
        private bool _requiresUpdate = false;

        public void _Initialise(IntVec3 dataPosition)
        {
            _voxelData = new VoxelData(dataPosition);
            _mesh = GetComponent<MeshFilter>().mesh;
            _collider = GetComponent<MeshCollider>();
        }

        public Block GetBlock(int x, int y, int z)
        {
            return new Block(_voxelData._GetData(x, y, z));
        }

        private void UpdateMesh()
        {
            _mesh.Clear();
            _mesh.vertices = _vertices;
            _mesh.triangles = _triangles;
            _mesh.uv = _uv;
            //_mesh.Optimise();
            _mesh.RecalculateNormals();

            _collider.sharedMesh = null;
            _collider.sharedMesh = _mesh;
        }

        private void LateUpdate()
        {
            if (_requiresUpdate)
            {
                UpdateMesh();
                _requiresUpdate = true;
            }
        }
    }
}