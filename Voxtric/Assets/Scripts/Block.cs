namespace VoxelEngine
{
    public struct Block
    {
        public byte ID;
        public byte transparent;
        public byte health;

        public Block(ushort data)
        {
            ID = (byte)(data & 255);
            transparent = (byte)((data & 256) >> 8);
            health = (byte)((data & 65024) >> 9);
        }

        public Block(byte ID, byte transparent, byte health)
        {
            this.ID = ID;
            this.transparent = transparent;
            this.health = health;
        }

        public static implicit operator ushort(Block block)
        {
            ushort data = (ushort)0;
            data = (ushort)((data | block.health) << 1);
            data = (ushort)((data | block.transparent) << 8);
            data = (ushort)(data | block.ID);
            return data;
        }
    }
}