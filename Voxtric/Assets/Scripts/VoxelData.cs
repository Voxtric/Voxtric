﻿using UnityEngine;
using System.IO;
using System.Text;
using System;

namespace VoxelEngine
{
    public sealed class VoxelData
    {
        public const int SIZE = 16;

        private ushort[,,] _data = new ushort[SIZE, SIZE, SIZE];
        private IntVec3 _dataPosition;

        public VoxelData(IntVec3 dataPosition)
        {
            _dataPosition = dataPosition;
        }

        public ushort _GetData(int x, int y, int z)
        {
            if (x < 0 || y < 0 || z < 0 || x >= SIZE || y >= SIZE || z >= SIZE)
            {
                Debug.Log(string.Format("Voxel data could not be retrieved so false value was provided: {0} is not a valid data position.", (string)_dataPosition));
                return (ushort)1;
            }
            return _data[x, y, z];
        }

        public void _SetData(int x, int y, int z, ushort data)
        {
            if (x < 0 || y < 0 || z < 0 || x >= SIZE || y >= SIZE || z >= SIZE)
            {
                Debug.Log(string.Format("Voxel data could not be set: {0} is not a valid data position.", (string)_dataPosition));
                return;
            }
            _data[x, y, z] = data;
        }

        public void _SaveData(string collectionPath)
        {
            string fullPath = string.Format(@"{0}\{1}.vdat", collectionPath, (string)_dataPosition);
            StringBuilder dataStore = new StringBuilder();
            for (int x = 0; x < SIZE; x++)
            {
                for (int y = 0; y < SIZE; y++)
                {
                    for (int z = 0; z < SIZE; z++)
                    {
                        dataStore.Append(_data[x, y, z].ToString("X4"));
                    }
                    dataStore.AppendLine();
                }
            }
            StreamWriter saveFile = new StreamWriter(fullPath);
            saveFile.Write(dataStore.ToString());
            saveFile.Close();
        }

        private void LoadData(string collectionPath)
        {
            string fullPath = string.Format(@"{0}\{1}.vdat", collectionPath, (string)_dataPosition);
            string dataLine;
            int index;
            if (File.Exists(fullPath))
            {
                StreamReader saveFile = new StreamReader(fullPath);
                for (int x = 0; x < SIZE; x++)
                {
                    for (int y = 0; y < SIZE; y++)
                    {
                        index = 0;
                        dataLine = saveFile.ReadLine();
                        for (int z = 0; z < SIZE; z++)
                        {
                            _data[x, y, z] = (ushort)(Convert.ToInt32(dataLine.Substring(index, 4), 16));
                        }
                    }
                }
                saveFile.Close();
            }
            //The following is purely for debugging purposes.
            else
            {
                for (int x = 0; x < SIZE; x++)
                {
                    for (int y = 0; y < SIZE; y++)
                    {
                        for (int z = 0; z < SIZE; z++)
                        {
                            if (y + (_dataPosition.y * SIZE) == 14)
                            {
                                _data[x, y, z] = 2;
                            }
                        }
                    }
                }
            }
        }
    }
}