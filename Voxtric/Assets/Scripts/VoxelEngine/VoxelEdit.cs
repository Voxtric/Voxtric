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
            return new Block();
        }

        public static void CheckCollectionSplit(RegionCollection regionCollection, List<IntVec3> points)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(CheckSplit), new SplitCheckInfo(regionCollection, points));
            //CheckSplit((System.Object)new SplitCheckInfo(regionCollection, points));
        }

        private static void CheckSplit(System.Object splitCheckInfo)
        {
            RegionCollection regionCollection = ((SplitCheckInfo)splitCheckInfo).regionCollection;
            List<IntVec3> points = ((SplitCheckInfo)splitCheckInfo).points;
            TrimBadPoints(regionCollection, points);

            List<IntVec3> toMove = new List<IntVec3>();
            int foundCollections = 0;
            bool split = false;

            while (points.Count > 0)
            {
                IntVec3 startingPoint = points[0];
                List<IntVec3> toCheck = new List<IntVec3> { startingPoint };
                List<IntVec3> newPoints = new List<IntVec3>();
                List<IntVec3> oldPoints = new List<IntVec3>();
                int iterations = 0;
                int checks = 0;
                while (toCheck.Count > 0)
                {
                    if (points.Count == 1 && !split)
                    {
                        toMove.Clear();
                        //Debug.Log(string.Format("Found all points with {0} iterations and {1} checks.", iterations, checks));
                        break;
                    }
                    else if (iterations == MAX_ITERATIONS)
                    {
                        split = true;
                        toMove.Clear();
                        //Debug.LogWarning(string.Format("Unable to complete split check after {0} iterations and {1} checks; attempting check from different point.", iterations, checks));
                        break;
                    }
                    iterations++;
                    foreach (IntVec3 point in toCheck)
                    {
                        checks++;
                        if (GetAt(regionCollection, point).visible == 1)
                        {
                            toMove.Add(point);
                            if (point != startingPoint && points.Contains(point))
                            {
                                points.Remove(point);
                            }
                            IntVec3 newPoint = point + IntVec3.right;
                            if (!toCheck.Contains(newPoint) && !newPoints.Contains(newPoint) && !oldPoints.Contains(newPoint))
                            {
                                newPoints.Add(newPoint);
                            }
                            newPoint = point + IntVec3.left;
                            if (!toCheck.Contains(newPoint) && !newPoints.Contains(newPoint) && !oldPoints.Contains(newPoint))
                            {
                                newPoints.Add(newPoint);
                            }
                            newPoint = point + IntVec3.forward;
                            if (!toCheck.Contains(newPoint) && !newPoints.Contains(newPoint) && !oldPoints.Contains(newPoint))
                            {
                                newPoints.Add(newPoint);
                            }
                            newPoint = point + IntVec3.back;
                            if (!toCheck.Contains(newPoint) && !newPoints.Contains(newPoint) && !oldPoints.Contains(newPoint))
                            {
                                newPoints.Add(newPoint);
                            }
                            newPoint = point + IntVec3.up;
                            if (!toCheck.Contains(newPoint) && !newPoints.Contains(newPoint) && !oldPoints.Contains(newPoint))
                            {
                                newPoints.Add(newPoint);
                            }
                            newPoint = point + IntVec3.down;
                            if (!toCheck.Contains(newPoint) && !newPoints.Contains(newPoint) && !oldPoints.Contains(newPoint))
                            {
                                newPoints.Add(newPoint);
                            }
                        }
                    }
                    oldPoints = toCheck;
                    toCheck = newPoints;
                    newPoints = new List<IntVec3>();
                }
                points.Remove(startingPoint);
                foundCollections++;
                if (toMove.Count > 0)
                {
                    Debug.Log(string.Format("{0} voxels to be moved.", toMove.Count));
                    ushort[] dataToMove = new ushort[toMove.Count];
                    for (int i = 0; i < toMove.Count; i++)
                    {
                        dataToMove[i] = (ushort)GetAt(regionCollection, toMove[i]);
                        SetAt(regionCollection, toMove[i], new Block());
                    }
                    regionCollection.SetPositionsToSplit(toMove.ToArray(), dataToMove);
                }
            }
            //Debug.Log(string.Format("Found {0} different collections.", foundCollections));
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