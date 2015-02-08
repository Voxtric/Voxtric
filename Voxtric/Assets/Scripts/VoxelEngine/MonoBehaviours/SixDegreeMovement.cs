using UnityEngine;

namespace VoxelEngine.MonoBehaviours
{
    public sealed class SixDegreeMovement : MonoBehaviour
    {
        private Transform _transform;
        private float _rotationY = 0f;
        private const float SPEED = 0.5f;

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
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                _transform.position += _transform.forward * (SPEED * 2);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                _transform.position -= _transform.forward * (SPEED * 2);
            }

            if (Input.GetKey(KeyCode.E))
            {
                _transform.position += _transform.up * SPEED;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                _transform.position -= _transform.up * SPEED;
            }
            if (Input.GetKey(KeyCode.W))
            {
                _transform.position += _transform.forward * SPEED;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _transform.position -= _transform.forward * SPEED;
            }
            if (Input.GetKey(KeyCode.D))
            {
                _transform.position += _transform.right * SPEED;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                _transform.position -= _transform.right * SPEED;
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