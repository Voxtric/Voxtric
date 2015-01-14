using UnityEngine;
using System.IO;

namespace VoxelEngine
{
    public sealed class ApplicationInitialiser : MonoBehaviour
    {
        public static string gamePath;

        private void Awake()
        {
            SetUpDirectories();
            TextureFinder.AssignAllTextureDetails();
        }

        private static void SetUpDirectories()
        {

        }
    }
}