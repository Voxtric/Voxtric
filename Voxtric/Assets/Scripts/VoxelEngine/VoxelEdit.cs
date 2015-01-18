using UnityEngine;
using VoxelEngine.MonoBehaviours;
using VoxelEngine.Hidden;

namespace VoxelEngine
{
    public static class VoxelEdit
    {
        public static IntVec3 WorldToDataPosition(RegionCollection regionCollection, Vector3 worldPosition)
        {
            Transform transform = regionCollection.GetPositionPointer();
            transform.position = worldPosition;
            return new IntVec3((int)transform.localPosition.x, (int)transform.localPosition.y, (int)transform.localPosition.z);
        }

        public static void ChangeAt(RegionCollection regionCollection, IntVec3 dataPosition, Block block)
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
            regionCollection.GetRegion(xRegionIndex, yRegionIndex, zRegionIndex).SetBlock(dataPosition.x, dataPosition.y, dataPosition.z, block);
        }
    }
}