using UnityEngine;

namespace VoxelEngine.Hidden
{
    public struct TextureDetails
    {
        public Vector2 origin;
        public Vector2 dimensions;

        public TextureDetails(Vector2 origin, Vector2 dimensions)
        {
            this.origin = origin;
            this.dimensions = dimensions;
        }
    }
}