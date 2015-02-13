using System.IO;
using UnityEngine;
using VoxelEngine.Hidden;

namespace VoxelEngine.MonoBehaviours
{
    public sealed class SceneInitialiser : MonoBehaviour
    {
        public static string gameDirectory;
        public static string sceneDirectory;
        public static string saveDirectory;

        private void Awake()
        {
            SetupDirectories();
            SetupEmptyRegion();
            TextureFinder.AssignAllTextureDetails();
            RegionCollection.regionCollectionPrefab = (GameObject)Resources.Load("Region Collection");
            RegionCollection.CreateRegionCollection(new Vector3(-20, 20, -20), Vector3.zero, new IntVec3(3, 3, 3), "Test Region 1");
        }

        private static void SetupDirectories()
        {
            gameDirectory = Application.persistentDataPath;
            sceneDirectory = string.Format(@"{0}\Scenes\{1}", gameDirectory, Application.loadedLevelName);
            saveDirectory = string.Format(@"{0}\Saves\{1}", gameDirectory, Application.loadedLevelName);
            Directory.CreateDirectory(string.Format(@"{0}\Textures", gameDirectory));
            Directory.CreateDirectory(sceneDirectory);
            Directory.CreateDirectory(string.Format(@"{0}\Collections", sceneDirectory));
        }

        private static void SetupEmptyRegion()
        {
            GameObject region = new GameObject("Empty region");
            Region.emptyRegion = region.AddComponent<Region>();
            region.hideFlags = HideFlags.HideInHierarchy;
        }
    }
}