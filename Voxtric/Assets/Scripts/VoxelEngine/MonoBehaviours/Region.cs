using UnityEngine;
using VoxelEngine.Hidden;
using System;

namespace VoxelEngine.MonoBehaviours
{
    public sealed class Region : MonoBehaviour
    {
        [SerializeField]
        private GameObject _concaveColliderPrefab = null;
        private ConcaveCollider _concaveCollider;

        private VoxelData _voxelData;
        private MeshGenerator _meshGenerator;
        private RegionCollection _regionCollection;

        private Vector3[] _vertices;
        private int[] _triangles;
        private Vector2[] _uv;

        private Mesh _mesh;
        private MeshCollider _convexCollider;
        private bool _requiresUpdate = false;
        private bool _blockChanged = false;

        public void LoadVoxelData(string collectionDirectory)
        {
            _voxelData.LoadData(collectionDirectory);
        }

        public void SaveVoxelData(string collectionDirectory)
        {
            _voxelData.SaveData(collectionDirectory);
        }

        public void GenerateMesh()
        {
            _meshGenerator.GenerateMesh(this);
        }

        public void Initialise(IntVec3 dataPosition, RegionCollection regionCollection)
        {
            gameObject.name = (string)dataPosition;
            renderer.material.mainTexture = TextureFinder.regionTexture;
            _voxelData = new VoxelData(dataPosition, regionCollection.collectionDirectory);
            _meshGenerator = new MeshGenerator();
            _mesh = GetComponent<MeshFilter>().mesh;
            _convexCollider = GetComponent<MeshCollider>();

            _regionCollection = regionCollection;
            transform.parent = regionCollection.transform;
            transform.localRotation = new Quaternion();
            transform.localPosition = (Vector3)dataPosition * VoxelData.SIZE;

            _concaveCollider = ((GameObject)Instantiate(_concaveColliderPrefab)).GetComponent<ConcaveCollider>();
            _concaveCollider.Initialise(dataPosition, regionCollection);
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
            _blockChanged = true;
        }

        private void UpdateMesh()
        {
            _mesh.Clear();
            _mesh.vertices = _vertices;
            _mesh.triangles = _triangles;
            _mesh.uv = _uv;
            //_mesh.Optimize();
            _mesh.RecalculateNormals();

            _convexCollider.sharedMesh = null;
            _convexCollider.sharedMesh = _mesh;
            _concaveCollider.UpdateCollider(_mesh, this);
        }

        public ColliderInfo GetColliderInfo()
        {
            return new ColliderInfo(_vertices, _triangles);
        }

        private void LateUpdate()
        {
            if (_requiresUpdate)
            {
                UpdateMesh();
                _requiresUpdate = false;
            }
            else if (_blockChanged)
            {
                GenerateMesh();
                _blockChanged = false;
            }
        }
    }
}