using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 120;

    [Header("Rotation details")]
    [SerializeField] private Transform focusPoint;
    [SerializeField] private float maxFocusPointDistance = 15f;
    [SerializeField] private float rotationSpeed = 200;
    
    [Space]
    
    private float _pitch;
    [SerializeField] private float _minPitch = 5f;
    [SerializeField] private float _maxPitch = 85f;
    
    [Header("Zoom details")]
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float _minZoom = 3f;
    [SerializeField] private float _maxZoom = 15f;

    private float _smoothTime = 0.1f;
    private Vector3 _movementVelocity = Vector3.zero;
    private Vector3 _zoomVelocity = Vector3.zero;
    
    void Update()
    {
        HandleRotation();
        HandleZoom();
        HandleMovement();
        
        focusPoint.position = transform.position + (transform.forward * GetFocusPointDistance());
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 zoomDirection = transform.forward * scroll * zoomSpeed;
        Vector3 targetPosition = transform.position + zoomDirection;

        if (transform.position.y < _minZoom && scroll > 0)
        {
            return;
        }

        if (transform.position.y > _maxZoom && scroll < 0)
        {
            return;
        }
        
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _zoomVelocity, _smoothTime);
    }

    private float GetFocusPointDistance()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxFocusPointDistance))
        {
            return hit.distance;
        }

        return maxFocusPointDistance;
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float verticalRotation = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            
            transform.RotateAround(focusPoint.position, Vector3.up, horizontalRotation);
            
            _pitch = Mathf.Clamp(_pitch - verticalRotation, _minPitch, _maxPitch);
            transform.RotateAround(focusPoint.position, transform.right, _pitch - transform.eulerAngles.x);

            transform.LookAt(focusPoint);
        }
    }

    private void HandleMovement()
    {
        Vector3 targetPosition = transform.position;
        
        float vInput = Input.GetAxisRaw("Vertical");
        float hInput = Input.GetAxisRaw("Horizontal");

        Vector3 flatForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        
        if (vInput > 0)
        {
            targetPosition += flatForward * movementSpeed * Time.deltaTime;
        }

        if (vInput < 0)
        {
            targetPosition -= flatForward * movementSpeed * Time.deltaTime;
        }
        
        if (hInput > 0)
        {
            targetPosition += transform.right * movementSpeed * Time.deltaTime;
        }

        if (hInput < 0)
        {
            targetPosition -= transform.right * movementSpeed * Time.deltaTime;
        }
        
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _movementVelocity, _smoothTime);
    }
}
