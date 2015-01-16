using UnityEngine;
using VoxelEngine.Hidden;

namespace VoxelEngine
{
    public sealed class RegionCollection : MonoBehaviour
    {
        [SerializeField]
        private GameObject regionPrefab = null;

        Region[,,] _regions;
        private IntVec3 _dimensions;

        private void Start()
        {
            Initialise(new IntVec3(3, 3, 3), "Test Region 1");
        }

        public void Initialise(IntVec3 dimensions, string name)
        {
            gameObject.name = name;
            _dimensions = dimensions;
            _regions = new Region[dimensions.x, dimensions.y, dimensions.z];

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