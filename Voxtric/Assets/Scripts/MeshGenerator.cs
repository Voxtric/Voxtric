using UnityEngine;
using System.Threading;
using System.Collections.Generic;

namespace VoxelEngine
{
    public static class MeshGenerator
    {
        public static void GenerateMesh(Region region)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(Generate), region);
        }

        private static void Generate(System.Object obj)
        {
            Region region = (Region)obj;
            region.QueueMeshUpdate();
        }
    }
}