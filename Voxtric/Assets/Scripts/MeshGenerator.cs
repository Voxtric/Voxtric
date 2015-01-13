using UnityEngine;
using System.Threading;
using System.Collections.Generic;

namespace VoxelEngine
{
    public sealed class MeshGenerator
    {
        public static List<MeshGenerator> activeGenerators = new List<MeshGenerator>();

        private List<Vector3> _vertices = new List<Vector3>();
        private List<int> _triangles = new List<int>();
        private List<Vector2> _uv = new List<Vector2>();
        private int _faceCount = 0;

        public MeshGenerator(Region region)
        {
            for (int x = 0; x < VoxelData.SIZE; x++)
            {
                for (int y = 0; y < VoxelData.SIZE; y++)
                {
                    for (int z = 0; z < VoxelData.SIZE; z++)
                    {
                        Block block = region.GetBlock(x, y, z);
                        if (block.transparent == 0)
                        {
                            //if (y )
                        }
                    }
                }
            }

            region.SetMeshInformation(_vertices.ToArray(), _triangles.ToArray(), _uv.ToArray());
        }

        public static void GenerateMesh(Region region)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(CreateNewGenerator), region);
        }

        private static void CreateNewGenerator(System.Object obj)
        {
            activeGenerators.Add(new MeshGenerator((Region)obj));
        }
    }
}