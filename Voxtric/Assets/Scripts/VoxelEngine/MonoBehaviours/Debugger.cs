using UnityEngine;
using VoxelEngine.Hidden;

namespace VoxelEngine.MonoBehaviours
{
    public class Debugger : MonoBehaviour
    {
        private bool _displayBoundaries = false;
        private bool _displayGUI = true;

        private void DisplayRegionBoundaries()
        {
            foreach (RegionCollection regionCollection in RegionCollection.allCollections)
            {
                IntVec3 dimensions = regionCollection.GetDimensions();
                for (int x = 0; x < dimensions.x; x++)
                {
                    for (int y = 0; y < dimensions.y; y++)
                    {
                        for (int z = 0; z < dimensions.z; z++)
                        {
                            Region region = regionCollection.GetRegion(x, y, z);
                            if (region != null)
                            {
                                Transform regionTransform = region.GetComponent<Transform>();
                                Vector3 position = regionTransform.position;
                                Vector3 up = regionTransform.up * VoxelData.SIZE;
                                Vector3 forward = regionTransform.forward * VoxelData.SIZE;
                                Vector3 right = regionTransform.right * VoxelData.SIZE;

                                Debug.DrawLine(position, position + up, Color.red);
                                Debug.DrawLine(position, position + forward, Color.red);
                                Debug.DrawLine(position, position + right, Color.red);
                                Debug.DrawLine(position + up, position + up + right, Color.red);
                                Debug.DrawLine(position + up, position + up + forward, Color.red);
                                Debug.DrawLine(position + up + right, position + up + right + forward, Color.red);
                                Debug.DrawLine(position + up + forward, position + up + forward + right, Color.red);
                                Debug.DrawLine(position + right, position + right + forward, Color.red);
                                Debug.DrawLine(position + forward, position + forward + right, Color.red);
                                Debug.DrawLine(position + forward, position + forward + up, Color.red);
                                Debug.DrawLine(position + right, position + right + up, Color.red);
                                Debug.DrawLine(position + right + forward, position + up + forward + right, Color.red);
                            }
                        }
                    }
                }
            }
        }

        private void SaveAllRegionCollections()
        {
            foreach (RegionCollection regionCollection in RegionCollection.allCollections)
            {
                regionCollection.SaveAllVoxelData();
            }
        }

        private void Update()
        {
            RegisterInputs();
            if (_displayBoundaries)
            {
                DisplayRegionBoundaries();
            }
        }

        private void OnGUI()
        {
            if (_displayGUI)
            {
                GUI.Label(new Rect(3, Screen.height - 23, 100, 20), string.Format("FPS: {0}", (int)(1.0f / Time.smoothDeltaTime)));
            }
        }

        private void RegisterInputs()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _displayBoundaries = !_displayBoundaries;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _displayGUI = !_displayGUI;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SaveAllRegionCollections();
            }
        }
    }
}