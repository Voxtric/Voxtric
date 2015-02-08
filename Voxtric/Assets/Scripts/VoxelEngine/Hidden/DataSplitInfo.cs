using System.Collections.Generic;

namespace VoxelEngine.Hidden
{
    public struct DataSplitInfo
    {
        public List<IntVec3> found;
        public HashSet<IntVec3> confirmed;

        public DataSplitInfo(List<IntVec3> found, HashSet<IntVec3> confirmed)
        {
            this.found = found;
            this.confirmed = confirmed;
        }
    }
}