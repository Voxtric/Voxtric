using UnityEngine;

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

        public static explicit operator string(IntVec3 intVec3) { return string.Format("X{0},Y{1},Z{2}", intVec3.x, intVec3.y, intVec3.z); }

        public static implicit operator Vector3(IntVec3 intVec3) { return new Vector3(intVec3.x, intVec3.y, intVec3.z); }

        public static IntVec3 operator +(IntVec3 first, IntVec3 second) { return new IntVec3(first.x + second.x, first.y + second.y, first.z + second.z); }
        public static IntVec3 operator -(IntVec3 first, IntVec3 second) { return new IntVec3(first.x - second.x, first.y - second.y, first.z - second.z); }

        public static IntVec3 one { get { return new IntVec3(1, 1, 1); } }
        public static IntVec3 east { get { return new IntVec3(1, 0, 0);} }
        public static IntVec3 west { get { return new IntVec3(-1, 0, 0); } }
        public static IntVec3 north { get { return new IntVec3(0, 0, 1); } }
        public static IntVec3 south { get { return new IntVec3(0, 0, -1); } }
        public static IntVec3 up { get { return new IntVec3(0, 1, 0); } }
        public static IntVec3 down { get { return new IntVec3(0, -1, 0); } }
    }
}