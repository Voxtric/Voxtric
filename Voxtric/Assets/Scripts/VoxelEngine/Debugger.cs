using UnityEngine;

namespace VoxelEngine
{
    public class Debugger : MonoBehaviour
    {
        private static bool _boundaryDisplay;

        private void Update()
        {
            RegisterInputs();

            if (_boundaryDisplay)
            {

            }
        }

        private void RegisterInputs()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                _boundaryDisplay = !_boundaryDisplay;
            }
        }
    }
}