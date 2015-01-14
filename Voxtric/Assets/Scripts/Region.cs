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

        private Mesh _mesh;
        private MeshCollider _collider;
        private bool _requiresUpdate = false;

        //For testing purposes.
        private void Start()
        {
            Initialise(new IntVec3());
            _voxelData.LoadData(Application.persistentDataPath);
            _voxelData.SaveData(Application.persistentDataPath);
            MeshGenerator.GenerateMesh(this);
        }

        public void Initialise(IntVec3 dataPosition)
        {
            renderer.material.mainTexture = TextureFinder.regionTexture;
            _voxelData = new VoxelData(dataPosition);
            _mesh = GetComponent<MeshFilter>().mesh;
            _collider = GetComponent<MeshCollider>();
        }

        public void SetMeshInformation(Vector3[] vertices, int[] triangles, Vector2[] uv)
        {
            _vertices = vertices;
            _triangles = triangles;
            _uv = uv;
            _requiresUpdate = true;
        }

        public Block GetBlock(int x, int y, int z)
        {
            return new Block(_voxelData.GetData(x, y, z));
        }

        public void SetBlock(int x, int y, int z, Block block)
        {
            _voxelData.SetData(x, y, z, (ushort)block);
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