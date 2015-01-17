using System.IO;
using UnityEngine;
using VoxelEngine.Hidden;

namespace VoxelEngine.MonoBehaviours
{
    public sealed class ApplicationInitialiser : MonoBehaviour
    {
        public static string gameDirectory;

        private void Awake()
        {
            SetUpDirectories();
            TextureFinder.AssignAllTextureDetails();
        }

        private static void SetUpDirectories()
        {
            gameDirectory = Application.persistentDataPath;
            Directory.CreateDirectory(string.Format(@"{0}\Textures", gameDirectory));
            Directory.CreateDirectory(string.Format(@"{0}\Collections", gameDirectory));
        }
    }
}