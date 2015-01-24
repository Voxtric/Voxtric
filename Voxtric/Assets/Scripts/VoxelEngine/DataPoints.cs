using VoxelEngine.Hidden;

namespace VoxelEngine
{
    public struct DataPoints
    {
        public readonly IntVec3 regionDataPosition;
        public readonly IntVec3 voxelDataPosition;

        public DataPoints(IntVec3 dataPosition)
        {
            int xRegionIndex = 0;
            int yRegionIndex = 0;
            int zRegionIndex = 0;
            while (dataPosition.x >= VoxelData.SIZE)
            {
                dataPosition.x -= VoxelData.SIZE;
                xRegionIndex++;
            }
            while (dataPosition.y >= VoxelData.SIZE)
            {
                dataPosition.y -= VoxelData.SIZE;
                yRegionIndex++;
            }
            while (dataPosition.z >= VoxelData.SIZE)
            {
                dataPosition.z -= VoxelData.SIZE;
                zRegionIndex++;
            }
            regionDataPosition = new IntVec3(xRegionIndex, yRegionIndex, zRegionIndex);
            voxelDataPosition = dataPosition;
        }
    }
}