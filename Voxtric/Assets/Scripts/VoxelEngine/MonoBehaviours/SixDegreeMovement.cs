using UnityEngine;

namespace VoxelEngine.MonoBehaviours
{
    public sealed class SixDegreeMovement : MonoBehaviour
    {
        private Transform _transform;
        private float _rotationY = 0f;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(1))
            {
                RegisterMovement();
                RegisterLooking();
                Screen.lockCursor = true;
                Screen.showCursor = false;
            }
            else
            {
                Screen.lockCursor = false;
                Screen.showCursor = true;
            }
        }

        private void RegisterMovement()
        {
            if (Input.GetKey(KeyCode.E))
            {
                _transform.position += _transform.up;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                _transform.position -= _transform.up;
            }
            if (Input.GetKey(KeyCode.W))
            {
                _transform.position += _transform.forward;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _transform.position -= _transform.forward;
            }
            if (Input.GetKey(KeyCode.D))
            {
                _transform.position += _transform.right;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                _transform.position -= _transform.right;
            }
        }

        private void RegisterLooking()
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * 7;
            _rotationY += Input.GetAxis("Mouse Y") * 7;
            transform.localEulerAngles = new Vector3(-_rotationY, rotationX, 0);
        }
    }
}