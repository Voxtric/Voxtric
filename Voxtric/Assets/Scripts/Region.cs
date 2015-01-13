using UnityEngine;

namespace VoxelEngine
{
    public sealed class Region : MonoBehaviour
    {
        private VoxelData _voxelData;

        public void _Initialise(IntVec3 dataPosition)
        {
            _voxelData = new VoxelData(dataPosition);
        }
    }
}