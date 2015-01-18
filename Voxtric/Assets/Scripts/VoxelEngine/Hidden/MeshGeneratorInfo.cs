using VoxelEngine.MonoBehaviours;

namespace VoxelEngine.Hidden
{
    public struct MeshGeneratorInfo
    {
        public Region region;
        public RegionCollection regionCollection;
        public IntVec3 dataPosition;

        public MeshGeneratorInfo(Region region, RegionCollection regionCollection, IntVec3 dataPosition)
        {
            this.region = region;
            this.regionCollection = regionCollection;
            this.dataPosition = dataPosition;
        }
    }
}