using VoxelEngine.MonoBehaviours;
using System.Collections.Generic;

namespace VoxelEngine.Hidden
{
    public struct SplitCheckInfo
    {
        public readonly RegionCollection regionCollection;
        public readonly List<IntVec3> points;

        public SplitCheckInfo(RegionCollection regionCollection, List<IntVec3> points)
        {
            this.regionCollection = regionCollection;
            this.points = points;
        }
    }
}