using UnityEngine;
using System.Threading;
using System.Collections.Generic;

namespace VoxelEngine
{
    public sealed class MeshGenerator //Entire class needs to be redone to be nested within region.
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
                        if (block.visible == 1)
                        {
                            if (x + 1 < VoxelData.SIZE)
                            {
                                if (region.GetBlock(x + 1, y, z).visible == 0)
                                {
                                    //CubeEast(x, y, z, block.ID);
                                }
                            }
                            if (x - 1 >= 0)
                            {
                                if (region.GetBlock(x - 1, y, z).visible == 0)
                                {
                                    //CubeWest(x, y, z, block.ID);
                                }
                            }
                            if (y + 1 < VoxelData.SIZE)
                            {
                                if (region.GetBlock(x, y + 1, z).visible == 0)
                                {
                                    //CubeTop(x, y, z, block.ID);
                                }
                            }
                            if (y - 1 >= 0)
                            {
                                if (region.GetBlock(x, y - 1, z).visible == 0)
                                {
                                    //CubeBottom(x, y, z, block.ID);
                                }
                            }
                            if (z + 1 < VoxelData.SIZE)
                            {
                                if (region.GetBlock(x, y, z + 1).visible == 0)
                                {
                                    //CubeNorth(x, y, z, block.ID);
                                }
                            }
                            if (z - 1 < VoxelData.SIZE)
                            {
                                if (region.GetBlock(x, y, z - 1).visible == 0)
                                {
                                    //CubeSouth(x, y, z, block.ID);
                                }
                            }
                        }
                    }
                }
            }
            region.SetMeshInformation(_vertices.ToArray(), _triangles.ToArray(), _uv.ToArray());
            activeGenerators.Remove(this);
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