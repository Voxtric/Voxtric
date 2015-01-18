using UnityEngine;
using VoxelEngine.Hidden;

namespace VoxelEngine.MonoBehaviours
{
    public sealed class ConcaveCollider : MonoBehaviour
    {
        MeshCollider _collider;
        RegionCollection _regionCollection;

        public void Initialise(IntVec3 dataPosition, RegionCollection regionCollection)
        {
            gameObject.name = (string)dataPosition + " Concave Collider";
            gameObject.layer = LayerMask.NameToLayer("Region Collection");
            _regionCollection = regionCollection;
            transform.parent = regionCollection.transform.GetChild(0);
            transform.localRotation = new Quaternion();
            transform.localPosition = (Vector3)dataPosition * VoxelData.SIZE;
            _collider = GetComponent<MeshCollider>();
        }

        public void UpdateCollider(Mesh mesh, Region region)
        {
            _collider.sharedMesh = null;
            _collider.sharedMesh = mesh;
            if (mesh.vertexCount > 0)
            {
                Physics.IgnoreCollision(_collider, region.collider);
            }
        }

        public RegionCollection GetRegionCollection()
        {
            return _regionCollection;
        }
    }
}