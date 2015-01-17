using UnityEngine;
using VoxelEngine.Hidden;

namespace VoxelEngine.MonoBehaviours
{
    public sealed class RegionCollection : MonoBehaviour
    {
        [SerializeField]
        private GameObject regionPrefab = null;

        Region[,,] _regions;
        private IntVec3 _dimensions;
        private MeshGenerator _meshGenerator;

        private bool _regionChanged = false;
        private bool _requiresUpdate = false;
        private Vector3[] _vertices;
        private int[] _triangles;

        private Mesh _mesh;
        private MeshCollider _collider;

        private void Start()
        {
            Initialise(new IntVec3(3, 3, 3), "Test Region 1");
        }

        public void QueueMeshGeneration()
        {
            _regionChanged = true;
        }

        public void SetMeshInformation(Vector3[] vertices, int[] triangles)
        {
            _vertices = vertices;
            _triangles = triangles;
            _requiresUpdate = true;
        }

        private void LateUpdate()
        {
            if (_requiresUpdate)
            {
                UpdateMesh();
                _requiresUpdate = false;
            }
            if (_regionChanged)
            {
                _meshGenerator.GenerateMesh(this);
                _regionChanged = false;
            }
        }

        public Region GetRegion(int x, int y, int z)
        {
            if (x < 0 || y < 0 || z < 0 || x >= _dimensions.x || y >= _dimensions.y || z >= _dimensions.z)
            {
                Debug.LogError(string.Format("Region could not be retrieved: {0} is not a valid data position.", new IntVec3(x, y, z)));
                return null;
            }
            return _regions[x, y, z];
        }

        public IntVec3 GetDimensions()
        {
            return _dimensions;
        }

        private void UpdateMesh()
        {
            _mesh.Clear();
            _mesh.vertices = _vertices;
            _mesh.triangles = _triangles;
            _collider.sharedMesh = null;
            _collider.sharedMesh = _mesh;
        }

        public void Initialise(IntVec3 dimensions, string name)
        {
            gameObject.name = name;
            _dimensions = dimensions;
            _regions = new Region[dimensions.x, dimensions.y, dimensions.z];
            _meshGenerator = new MeshGenerator();
            _mesh = new Mesh();
            _collider = GetComponent<MeshCollider>();

            for (int x = 0; x < dimensions.x; x++)
            {
                for (int y = 0; y < dimensions.y; y++)
                {
                    for (int z = 0; z < dimensions.z; z++)
                    {
                        _regions[x, y, z] = CreateRegion(new IntVec3(x, y, z));
                    }
                }
            }

            for (int x = 0; x < dimensions.x; x++)
            {
                for (int y = 0; y < dimensions.y; y++)
                {
                    for (int z = 0; z < dimensions.z; z++)
                    {
                        _regions[x, y, z].GenerateMesh();
                    }
                }
            }
        }

        public Region CreateRegion(IntVec3 dataPosition)
        {
            Region region = ((GameObject)Instantiate(regionPrefab)).GetComponent<Region>();
            region.Initialise(dataPosition, this);
            return region;
        }
    }
}