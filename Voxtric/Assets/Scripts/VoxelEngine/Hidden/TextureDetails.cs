namespace VoxelEngine.Hidden
{
    public struct TextureDetails
    {
        public readonly IntVec2 origin;
        public readonly IntVec2 dimensions;

        public TextureDetails(IntVec2 origin, IntVec2 dimensions)
        {
            this.origin = origin;
            this.dimensions = dimensions;
        }
    }
}