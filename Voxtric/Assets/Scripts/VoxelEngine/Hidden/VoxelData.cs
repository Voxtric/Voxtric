using UnityEngine;
using System.IO;
using System.Text;
using System;

namespace VoxelEngine.Hidden
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

        public ushort GetData(int x, int y, int z)
        {
            if (x < 0 || y < 0 || z < 0 || x >= SIZE || y >= SIZE || z >= SIZE)
            {
                Debug.LogError(string.Format("Voxel data could not be retrieved so false value was provided: {0} is not a valid data position.", new IntVec3(x, y, z)));
                return (ushort)1;
            }
            return _data[x, y, z];
        }

        public void SetData(int x, int y, int z, ushort data)
        {
            if (x < 0 || y < 0 || z < 0 || x >= SIZE || y >= SIZE || z >= SIZE)
            {
                Debug.LogError(string.Format("Voxel data could not be set: {0} is not a valid data position.", _dataPosition));
                return;
            }
            _data[x, y, z] = data;
        }

        public void SaveData(string collectionPath)
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

        public void LoadData(string collectionPath)
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
                            _data[x, y, z] = (Convert.ToUInt16(dataLine.Substring(index, 4), 16));
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
                                _data[x, y, z] = (ushort)new Block(2, 1, 0);
                            }
                        }
                    }
                }
            }
        }
    }
}