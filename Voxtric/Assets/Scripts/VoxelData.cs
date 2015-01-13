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
    }
}