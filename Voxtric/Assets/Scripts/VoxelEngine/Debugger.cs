using UnityEngine;

namespace VoxelEngine.MonoBehaviours
{
    public class Debugger : MonoBehaviour
    {
        private bool _displayBoundary = false;
        private bool _displayGUI = true;

        private void Update()
        {
            RegisterInputs();

            if (_displayBoundary)
            {

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
            if (Input.GetKeyDown(KeyCode.B))
            {
                _displayBoundary = !_displayBoundary;
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                _displayGUI = !_displayGUI;
            }
        }
    }
}