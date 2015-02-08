using UnityEngine;
using VoxelEngine.Hidden;
using System.Collections.Generic;

namespace VoxelEngine.MonoBehaviours
{
    public class Debugger : MonoBehaviour
    {
        private GUIStyle _style = new GUIStyle();

        private bool _displayBoundaries = false;
        private bool _displayGUI = true;
        private byte _damage = 50;
        private byte _radius = 10;

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
                            if (region != null && region != Region.emptyRegion)
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

        private void Awake()
        {
            _style.normal.textColor = Color.magenta;
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
                GUI.Label(new Rect(3, Screen.height - 23, 100, 20), string.Format("FPS: {0}\n", (int)(1.0f / Time.smoothDeltaTime)), _style);
                int totalLoaded = RegionCollection.totalLoadedRegions;
                GUI.Label(new Rect(3, 3, 300, 300), string.Format("Active region collection: {0}\nRegions in memory: {1} ({2} voxels)\n\nDamage: {3}\nRadius: {4}", RegionCollection.allCollections.Count, totalLoaded, totalLoaded * Mathf.Pow(VoxelData.SIZE, 3), _damage, _radius), _style);
            }
        }

        private void ChangeAtCursor()
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Region Collection")))
            {
                RegionCollection regionCollection = hit.collider.GetComponent<ConcaveCollider>().GetRegionCollection();
                Vector3 position = hit.point + (hit.normal * -0.5f) + regionCollection.transform.GetChild(0).up;
                IntVec3 changePosition = VoxelEdit.WorldToDataPosition(regionCollection, position);
                VoxelEdit.DamageAt(regionCollection, changePosition, _damage, _radius);
            }
        }

        private void RegisterInputs()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ChangeAtCursor();
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                _radius++;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                _radius--;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                _damage++;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                _damage--;
            }

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