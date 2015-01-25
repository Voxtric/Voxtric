using UnityEngine;
using VoxelEngine.MonoBehaviours;
using VoxelEngine.Hidden;
using System.Collections.Generic;
using System.Threading;
using System;

namespace VoxelEngine
{
    public static class VoxelEdit
    {
        private const int MAX_ITERATIONS = 25;

        public static IntVec3 WorldToDataPosition(RegionCollection regionCollection, Vector3 worldPosition)
        {
            Transform transform = regionCollection.GetPositionPointer();
            transform.position = worldPosition;
            return new IntVec3((int)transform.localPosition.x, (int)transform.localPosition.y, (int)transform.localPosition.z);
        }

        public static void SetAt(RegionCollection regionCollection, IntVec3 dataPosition, Block block)
        {
            DataPoints points = new DataPoints(dataPosition);
            Region region = regionCollection.GetRegion(points.regionDataPosition.x, points.regionDataPosition.y, points.regionDataPosition.z);
            if (!ReferenceEquals(region, null))
            {
                region.SetBlock(points.voxelDataPosition.x, points.voxelDataPosition.y, points.voxelDataPosition.z, block);
            }
        }

        public static Block GetAt(RegionCollection regionCollection, IntVec3 dataPosition)
        {
            DataPoints points = new DataPoints(dataPosition);
            Region region = regionCollection.GetRegion(points.regionDataPosition.x, points.regionDataPosition.y, points.regionDataPosition.z);
            if (!ReferenceEquals(region, null))
            {
                return region.GetBlock(points.voxelDataPosition.x, points.voxelDataPosition.y, points.voxelDataPosition.z);
            }
            return new Block(0, 1, 0);
        }

        public static void CheckCollectionSplit(RegionCollection regionCollection, List<IntVec3> points)
        {
            //ThreadPool.QueueUserWorkItem(new WaitCallback(CheckSplit), new SplitCheckInfo(regionCollection, points));
            CheckSplit((System.Object)new SplitCheckInfo(regionCollection, points));
        }

        private static void CheckSplit(System.Object splitCheckInfo)
        {
            RegionCollection regionCollection = ((SplitCheckInfo)splitCheckInfo).regionCollection;
            List<IntVec3> startPositions = ((SplitCheckInfo)splitCheckInfo).positions;
            TrimBadPoints(regionCollection, startPositions);
            List<DataSplitFinder> finders = new List<DataSplitFinder>();
            List<DataSplitFinder> findersToRemove = new List<DataSplitFinder>();
            foreach (IntVec3 position in startPositions)
            {
                finders.Add(new DataSplitFinder(position, regionCollection, finders, findersToRemove));
            }

            int iterationCalls = 0;
            while (finders.Count > 0)
            {
                foreach (DataSplitFinder finder in finders)
                {
                    if (!findersToRemove.Contains(finder))
                    {
                        iterationCalls++;
                        finder.Iterate();
                    }
                }
                foreach (DataSplitFinder finder in findersToRemove)
                {
                    finders.Remove(finder);
                }
            }
            Debug.Log(string.Format("{0} iteraction calls made.", iterationCalls));

            //regionCollection.SetPositionsToSplit(positionsToMove, dataToMove);
        }

        private static void TrimBadPoints(RegionCollection regionCollection, List<IntVec3> points)
        {
            for (int i = 0; i < points.Count; i++)
            {
                IntVec3 point = points[i];
                IntVec3 dimensions = regionCollection.GetDimensions() * VoxelData.SIZE;
                if (point.x < 0 || point.y < 0 || point.z < 0 || point.x >= dimensions.x || point.y >= dimensions.y || point.z >= dimensions.z || GetAt(regionCollection, point).visible == 0)
                {
                    points.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}