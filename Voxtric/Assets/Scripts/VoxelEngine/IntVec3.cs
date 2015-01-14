namespace VoxelEngine
{
    public struct IntVec3
    {
        public int x;
        public int y;
        public int z;

        public IntVec3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator string(IntVec3 position)
        {
            return string.Format("X{0},Y{1},Z{2}", position.x, position.y, position.z);
        }
    }
}