using UnityEngine;
using VoxelEngine.Hidden;
using System;

namespace VoxelEngine.MonoBehaviours
{
    public sealed class Region : MonoBehaviour
    {
        public static Region emptyRegion;

        [SerializeField]
        private GameObject _concaveColliderPrefab = null;
        private ConcaveCollider _concaveCollider;

        private VoxelData _voxelData;
        private MeshGenerator _meshGenerator;
        private RegionCollection _regionCollection;

        private Vector3[] _vertices;
        private int[] _triangles;
        private Vector2[] _uv;
        private int _blocks;

        private Mesh _mesh;
        private MeshCollider _convexCollider;
        private bool _requiresUpdate = false;
        private bool _requiresGeneration = false;

        public void LoadVoxelData(string collectionDirectory)
        {
            if (!ReferenceEquals(this, Region.emptyRegion))
            {
                _voxelData.LoadData(collectionDirectory);
            }
        }

        public void SaveVoxelData(string collectionDirectory)
        {
            if (!ReferenceEquals(this, Region.emptyRegion))
            {
                _voxelData.SaveData(collectionDirectory);
            }
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
            transform.parent = regionCollection.transform.GetChild(1);
            transform.localRotation = new Quaternion();
            transform.localPosition = (Vector3)dataPosition * VoxelData.SIZE;

            _concaveCollider = ((GameObject)Instantiate(_concaveColliderPrefab)).GetComponent<ConcaveCollider>();
            _concaveCollider.Initialise(dataPosition, regionCollection);
        }

        public void DestroyConcaveCollider()
        {
            MonoBehaviour.Destroy(_concaveCollider.gameObject);
        }

        public void SetMeshInformation(Vector3[] vertices, int[] triangles, Vector2[] uv, int blocks)
        {
            _vertices = vertices;
            _triangles = triangles;
            _uv = uv;
            _blocks = blocks;
            _requiresUpdate = true;
        }

        public Block GetBlock(int x, int y, int z)
        {
            return new Block(_voxelData.GetData(x, y, z));
        }

        public void SetBlock(int x, int y, int z, Block block)
        {
            _voxelData.SetData(x, y, z, (ushort)block);
            QueueMeshGeneration();

            Region region;
            IntVec3 dataPosition = _voxelData.GetDataPosition();
            if (x == 0)
            {
                if (dataPosition.x > 0)
                {
                    region = _regionCollection.GetRegion(dataPosition.x - 1, dataPosition.y, dataPosition.z);
                    if (!ReferenceEquals(region, null))
                    {
                        region.QueueMeshGeneration();
                    }
                    else
                    {
                        region = _regionCollection.CreateRegion(dataPosition.x - 1, dataPosition.y, dataPosition.z);
                        region.QueueMeshGeneration();
                    }
                }
            }
            else if (x == VoxelData.SIZE - 1 && dataPosition.x < _regionCollection.GetDimensions().x - 1)
            {
                region = _regionCollection.GetRegion(dataPosition.x + 1, dataPosition.y, dataPosition.z);
                if (!ReferenceEquals(region, null))
                {
                    region.QueueMeshGeneration();
                }
                else
                {
                    region = _regionCollection.CreateRegion(dataPosition.x + 1, dataPosition.y, dataPosition.z);
                    region.QueueMeshGeneration();
                }
            }
            if (y == 0)
            {
                if (dataPosition.y > 0)
                {
                    region = _regionCollection.GetRegion(dataPosition.x, dataPosition.y - 1, dataPosition.z);
                    if (!ReferenceEquals(region, null))
                    {
                        region.QueueMeshGeneration();
                    }
                    else
                    {
                        region = _regionCollection.CreateRegion(dataPosition.x, dataPosition.y - 1, dataPosition.z);
                        region.QueueMeshGeneration();
                    }
                }
            }
            else if (y == VoxelData.SIZE - 1 && dataPosition.y < _regionCollection.GetDimensions().y - 1)
            {
                region = _regionCollection.GetRegion(dataPosition.x, dataPosition.y + 1, dataPosition.z);
                if (!ReferenceEquals(region, null))
                {
                    region.QueueMeshGeneration();
                }
                else
                {
                    region = _regionCollection.CreateRegion(dataPosition.x, dataPosition.y + 1, dataPosition.z);
                    region.QueueMeshGeneration();
                }
            }
            if (z == 0)
            {
                if (dataPosition.z > 0)
                {
                    region = _regionCollection.GetRegion(dataPosition.x, dataPosition.y, dataPosition.z - 1);
                    if (!ReferenceEquals(region, null))
                    {
                        region.QueueMeshGeneration();
                    }
                    else
                    {
                        region = _regionCollection.CreateRegion(dataPosition.x, dataPosition.y, dataPosition.z - 1);
                        region.QueueMeshGeneration();
                    }
                }
            }
            else if (z == VoxelData.SIZE - 1 && dataPosition.z < _regionCollection.GetDimensions().z - 1)
            {
                region = _regionCollection.GetRegion(dataPosition.x, dataPosition.y, dataPosition.z + 1);
                if (!ReferenceEquals(region, null))
                {
                    region.QueueMeshGeneration();
                }
                else
                {
                    region = _regionCollection.CreateRegion(dataPosition.x, dataPosition.y, dataPosition.z + 1);
                    region.QueueMeshGeneration();
                }
            }
        }

        public void QueueMeshGeneration()
        {
            _requiresGeneration = true;
        }

        private void UpdateMesh()
        {
            if (_vertices.Length == 0)
            {
                if (_blocks == 0)
                {
                    _regionCollection.UnloadRegion(_voxelData.GetDataPosition(), true);
                }
                else
                {
                    _regionCollection.UnloadRegion(_voxelData.GetDataPosition(), false);
                }
            }
            else
            {
                _mesh.Clear();
                _mesh.vertices = _vertices;
                _mesh.triangles = _triangles;
                _mesh.uv = _uv;
                //_mesh.Optimize();
                _mesh.RecalculateNormals();

                _convexCollider.sharedMesh = null;
                _convexCollider.sharedMesh = _mesh;
                _concaveCollider.UpdateCollider(_mesh);
            }
        }

        private void LateUpdate()
        {
            if (_requiresUpdate)
            {
                UpdateMesh();
                _requiresUpdate = false;
            }
            else if (_requiresGeneration && this != emptyRegion)
            {
                _meshGenerator.GenerateMesh(new MeshGeneratorInfo(this, _regionCollection, _voxelData.GetDataPosition()));
                _requiresGeneration = false;
            }
        }
    }
}