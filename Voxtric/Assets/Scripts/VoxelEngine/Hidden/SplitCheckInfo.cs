using VoxelEngine.MonoBehaviours;
using System.Collections.Generic;

namespace VoxelEngine.Hidden
{
    public struct SplitCheckInfo
    {
        public readonly RegionCollection regionCollection;
        public readonly List<IntVec3> positions;

        public SplitCheckInfo(RegionCollection regionCollection, List<IntVec3> positions)
        {
            this.regionCollection = regionCollection;
            this.positions = positions;
        }
    }
}