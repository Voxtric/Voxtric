using UnityEngine;
using VoxelEngine.Hidden;

namespace VoxelEngine.MonoBehaviours
{
    public sealed class ApplicationInitialiser : MonoBehaviour
    {
        public static string gamePath;

        private void Awake()
        {
            gamePath = Application.persistentDataPath;
            SetUpDirectories();
            TextureFinder.AssignAllTextureDetails();
        }

        private static void SetUpDirectories()
        {

        }
    }
}