using UnityEngine;
using System;

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

        public static IntVec3 ReplaceFewer(IntVec3 intVec3_1, IntVec3 intVec3_2)
        {
            IntVec3 fewest = intVec3_1;
            if (intVec3_2.x < fewest.x)
            {
                fewest.x = intVec3_2.x;
            }
            if (intVec3_2.y < fewest.y)
            {
                fewest.y = intVec3_2.y;
            }
            if (intVec3_2.z < fewest.z)
            {
                fewest.z = intVec3_2.z;
            }
            return fewest;
        }

        public static IntVec3 ReplaceGreater(IntVec3 intVec3_1, IntVec3 intVec3_2)
        {
            IntVec3 greatest = intVec3_1;
            if (intVec3_2.x > greatest.x)
            {
                greatest.x = intVec3_2.x;
            }
            if (intVec3_2.y > greatest.y)
            {
                greatest.y = intVec3_2.y;
            }
            if (intVec3_2.z > greatest.z)
            {
                greatest.z = intVec3_2.z;
            }
            return greatest;
        }

        public static explicit operator string(IntVec3 intVec3) { return string.Format("X{0},Y{1},Z{2}", intVec3.x, intVec3.y, intVec3.z); }
        public static implicit operator Vector3(IntVec3 intVec3) { return new Vector3(intVec3.x, intVec3.y, intVec3.z); }

        public static IntVec3 one { get { return new IntVec3(1, 1, 1); } }
        public static IntVec3 right { get { return new IntVec3(1, 0, 0); } }
        public static IntVec3 left { get { return new IntVec3(-1, 0, 0); } }
        public static IntVec3 forward { get { return new IntVec3(0, 0, 1); } }
        public static IntVec3 back { get { return new IntVec3(0, 0, -1); } }
        public static IntVec3 up { get { return new IntVec3(0, 1, 0); } }
        public static IntVec3 down { get { return new IntVec3(0, -1, 0); } }

        public static IntVec3 operator +(IntVec3 intVec3_1, IntVec3 intVec3_2) { return new IntVec3(intVec3_1.x + intVec3_2.x, intVec3_1.y + intVec3_2.y, intVec3_1.z + intVec3_2.z); }
        public static IntVec3 operator -(IntVec3 intVec3_1, IntVec3 intVec3_2) { return new IntVec3(intVec3_1.x - intVec3_2.x, intVec3_1.y - intVec3_2.y, intVec3_1.z - intVec3_2.z); }
        public static IntVec3 operator *(IntVec3 intVec, int num) { return new IntVec3(intVec.x * num, intVec.y * num, intVec.z * num); }

        public static bool operator ==(IntVec3 intVec3_1, IntVec3 intVec3_2)
        {
            if (System.Object.ReferenceEquals(intVec3_1, intVec3_2))
            {
                return true;
            }
            else if ((object)intVec3_1 == null || (object)intVec3_2 == null)
            {
                return false;
            }
            return intVec3_1.x == intVec3_2.x && intVec3_1.y == intVec3_2.y && intVec3_1.z == intVec3_2.z;
        }

        public static bool operator !=(IntVec3 intVec3_1, IntVec3 intVec3_2)
        {
            return !(intVec3_1 == intVec3_2);
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            IntVec3 intVec3 = (IntVec3)obj;
            if ((System.Object)intVec3 == null)
            {
                return false;
            }
            return x == intVec3.x && y == intVec3.y && z == intVec3.z;
        }
        
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}