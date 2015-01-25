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
            RegionCollection.regionCollectionPrefab = (GameObject)Resources.Load("Region Collection");
            RegionCollection.CreateRegionCollection(new Vector3(-20, 20, -20), Vector3.zero, new IntVec3(3, 3, 3), "Test Region 1");
        }

        private static void SetUpDirectories()
        {
            gameDirectory = Application.persistentDataPath;
            Directory.CreateDirectory(string.Format(@"{0}\Textures", gameDirectory));
            Directory.CreateDirectory(string.Format(@"{0}\Collections", gameDirectory));
        }
    }
}