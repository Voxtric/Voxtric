namespace VoxelEngine
{
    public struct Block
    {
        public int ID;
        public int transparent;
        public int health;

        public Block(ushort data)
        {
            ID = data & 255;
            transparent = (data & 256) >> 8;
            health = (data & 65024) >> 9;
        }
    }
}