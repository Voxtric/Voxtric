using UnityEngine;

namespace VoxelEngine
{
    public struct IntVec2
    {
        public int x;
        public int y;

        public IntVec2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator Vector2(IntVec2 intVec2)
        {
            return new Vector2(intVec2.x, intVec2.y);
        }
    }
}