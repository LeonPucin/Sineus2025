using UnityEngine;

namespace Game.Gameplay.CameraControls
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _resetRotation;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _distance = 5.0f;
        [SerializeField] private float _rotationSpeed = 120.0f;
        [SerializeField] private float _minDistance = 2.0f;
        [SerializeField] private float _maxDistance = 10.0f;
        [SerializeField] private float _minVerticalAngle = -90.0f; // Минимальный угол поворота вверх
        [SerializeField] private float _maxVerticalAngle = 90.0f;  // Максимальный угол поворота вниз
        [SerializeField] private float _scrollSpeed = 2.0f; // Скорость изменения дистанции при скролле
        [SerializeField] private LayerMask _collisionLayers;
        
        private Vector3 _targetOffsetted => _target.TransformPoint(_offset);
        
        private Vector3 _currentRotation;
        private bool _isRotating;
        private float _targetDistance;

        public void SetRotationSpeed(float value)
        {
            _rotationSpeed = value;
        }
        
        private void OnEnable()
        {
            _targetDistance = _distance;
        }

        private void LateUpdate()
        {
            // _isRotating = _inputProvider.IsAimPressed();
            // float scrollValue = _inputProvider.GetScrollDelta();
            
            if (_isRotating)
            {
                // RotateCamera(_inputProvider.GetAimDirection());
                RotateCameraPosition();
            }
            else
            {
                FollowTarget();
            }

            // AdjustCameraDistance(scrollValue);
            RaycastToAvoidClipping();
        }

        private void RotateCamera(Vector2 mouseInput)
        {
            if (!_isRotating) return;
            
            _currentRotation.x += -mouseInput.y * _rotationSpeed;
            _currentRotation.y += mouseInput.x * _rotationSpeed;

            // Ограничение угла по оси X (вертикальное вращение)
            _currentRotation.x = Mathf.Clamp(_currentRotation.x, _minVerticalAngle, _maxVerticalAngle);
        }

        private void RotateCameraPosition()
        {
            Quaternion rotation = Quaternion.Euler(_currentRotation.x, _currentRotation.y, 0);
            Vector3 desiredPosition = _targetOffsetted - rotation * Vector3.forward * _distance;

            transform.position = desiredPosition;
            transform.LookAt(_targetOffsetted);
        }

        private void FollowTarget()
        {
            Quaternion rotation = Quaternion.Euler(_currentRotation.x, _currentRotation.y, 0);
            Vector3 followPosition = _targetOffsetted - rotation * Vector3.forward * _distance;

            transform.position = followPosition;
            transform.LookAt(_targetOffsetted);
        }

        private void RaycastToAvoidClipping()
        {
            Vector3 direction = (transform.position - _targetOffsetted).normalized;
            if (Physics.Raycast(_targetOffsetted, direction, out RaycastHit hit, _distance, _collisionLayers))
            {
                float distanceToHit = Vector3.Distance(_targetOffsetted, hit.point);
                _distance = Mathf.Clamp(distanceToHit, _minDistance, _targetDistance);
            }
            else
            {
                _distance = Mathf.Lerp(_distance, _targetDistance, Time.deltaTime * 5f);
            }
        }
        
        private void AdjustCameraDistance(float scrollValue)
        {
            _targetDistance -= scrollValue * _scrollSpeed * Time.deltaTime;
            _targetDistance = Mathf.Clamp(_targetDistance, _minDistance, _maxDistance);

            RaycastToAvoidClipping();
        }

        public void Reset()
        {
            _currentRotation = _resetRotation;
        }
    }
}