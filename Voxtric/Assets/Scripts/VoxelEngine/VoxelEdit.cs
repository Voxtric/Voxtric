using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VoxelEngine.Hidden;
using VoxelEngine.MonoBehaviours;

namespace VoxelEngine
{
    public static class VoxelEdit
    {
        private const int MAX_ITERATIONS = 100;

        public static IntVec3 WorldToDataPosition(RegionCollection regionCollection, Vector3 worldPosition)
        {
            Transform transform = regionCollection.GetPositionPointer();
            transform.position = worldPosition;
            return new IntVec3((int)transform.localPosition.x, (int)transform.localPosition.y, (int)transform.localPosition.z);
        }

        public static bool ValidPosition(IntVec3 dimensions, IntVec3 position)
        {
            if (position.x >= 0 && position.y >= 0 && position.z >= 0 && position.x < dimensions.x && position.y < dimensions.y && position.z < dimensions.z)
            {
                return true;
            }
            return false;
        }

        public static bool BrokeWithDamageAt(RegionCollection regionCollection, IntVec3 dataPosition, byte damage)
        {
            DataPoints points = new DataPoints(dataPosition);
            Region region = regionCollection.GetRegion(points.regionDataPosition.x, points.regionDataPosition.y, points.regionDataPosition.z);
            return region.BrokeBlockWithDamage(points.voxelDataPosition.x, points.voxelDataPosition.y, points.voxelDataPosition.z, damage);
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
            ThreadPool.QueueUserWorkItem(new WaitCallback(CheckSplit), new SplitCheckInfo(regionCollection, points));
            //CheckSplit((System.Object)new SplitCheckInfo(regionCollection, points));
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
            if (finders.Count > 1)
            {
                DataSplitFinder.SortFindersList(finders);
            }
            DataSplitFinder iteratingFinder;
            while (finders.Count > 1)
            {
                if (iterationCalls > MAX_ITERATIONS)
                {
                    string voxelCounts = "";
                    foreach (DataSplitFinder finder in finders)
                    {
                        voxelCounts = string.Format("{0}{1}, ", voxelCounts, finder.GetVoxelCount());
                    }
                    throw new OperationCanceledException(string.Format("Split too large to calculate with voxel counts of {0} and iteration count of {1}.", voxelCounts.Substring(0, voxelCounts.Length - 3), iterationCalls));
                }
                iteratingFinder = finders[0];
                iteratingFinder.Iterate();
                iterationCalls++;
                if (finders.Count > 1 && iteratingFinder.GetVoxelCount() > finders[1].GetVoxelCount())
                {
                    DataSplitFinder.SortFindersList(finders);
                }

                foreach (DataSplitFinder finder in findersToRemove)
                {
                    finders.Remove(finder);
                }
            }
        }

        private static void TrimBadPoints(RegionCollection regionCollection, List<IntVec3> positions)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                IntVec3 position = positions[i];
                if (GetAt(regionCollection, position).visible == 0)
                {
                    positions.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}