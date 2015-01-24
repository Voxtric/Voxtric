using UnityEngine;
using VoxelEngine.MonoBehaviours;
using VoxelEngine.Hidden;
using System.Collections.Generic;
using System.Threading;

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
            DataPoints points = new DataPoints(dataPosition);
            regionCollection.GetRegion(points.regionDataPosition.x, points.regionDataPosition.y, points.regionDataPosition.z).SetBlock(points.voxelDataPosition.x, points.voxelDataPosition.y, points.voxelDataPosition.z, block);
        }

        public static Block GetAt(RegionCollection regionCollection, IntVec3 dataPosition)
        {
            DataPoints points = new DataPoints(dataPosition);
            return regionCollection.GetRegion(points.regionDataPosition.x, points.regionDataPosition.y, points.regionDataPosition.z).GetBlock(points.voxelDataPosition.x, points.voxelDataPosition.y, points.voxelDataPosition.z);
        }

        public static void CheckCollectionSplit(RegionCollection regionCollection, List<IntVec3> points)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(CheckSplit), new SplitCheckInfo(regionCollection, points));
        }

        private static void CheckSplit(System.Object splitCheckInfo)
        {
            SplitCheckInfo info = (SplitCheckInfo)splitCheckInfo;
            TrimUnwantedPoints(info.regionCollection, info.points);
            Debug.Log(info.points.Count);
        }

        private static void TrimUnwantedPoints(RegionCollection regionCollection, List<IntVec3> points)
        {
            for (int i = 0; i < points.Count; i++)
            {
                IntVec3 point = points[i];
                IntVec3 dimensions = regionCollection.GetDimensions() * VoxelData.SIZE;
                if (point.x < 0 || point.y < 0 || point.z < 0 || point.x >= dimensions.x || point.y >= dimensions.y || point.z >= dimensions.z)
                {
                    points.RemoveAt(i);
                    i--;
                }
                else if (GetAt(regionCollection, point).visible == 0)
                {
                    points.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}