using UnityEngine;

namespace VoxelEngine
{
    public sealed class RegionCollection : MonoBehaviour
    {
        Region[,,] _regions;
        private IntVec3 _dimensions;

        public void Initialise(IntVec3 dimensions, string name)
        {
            gameObject.name = name;
            _dimensions = dimensions;
            _regions = new Region[dimensions.x, dimensions.y, dimensions.z];
        }
    }
}