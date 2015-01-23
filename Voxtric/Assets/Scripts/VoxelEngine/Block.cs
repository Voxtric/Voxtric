namespace VoxelEngine
{
    public struct Block
    {
        public readonly byte ID;
        public readonly byte visible;
        public byte health;

        public Block(ushort data)
        {
            ID = (byte)(data & 255);
            visible = (byte)((data & 256) >> 8);
            health = (byte)((data & 65024) >> 9);
        }

        public Block(byte ID, byte transparent, byte health)
        {
            this.ID = ID;
            this.visible = transparent;
            this.health = health;
        }

        public static implicit operator ushort(Block block)
        {
            ushort data = (ushort)0;
            data = (ushort)((data | block.health) << 1);
            data = (ushort)((data | block.visible) << 8);
            data = (ushort)(data | block.ID);
            return data;
        }

        public static explicit operator string(Block block)
        {
            return string.Format("ID:{0}, Visible:{1}, Health:{2}", block.ID, block.visible, block.health);
        }
    }
}