using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement Details")]
    [SerializeField] private float movementSpeed = 120;
    [SerializeField] private float mouseMovementSpeed = 120;
    [SerializeField] private float edgeMovementSpeed = 120;
    [SerializeField] private float edgeTreshold = 10;
    private float _screenWidth;
    private float _screenHeight;
    
    

    
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
    private Vector3 _mouseMovementVelocity = Vector3.zero;
    private Vector3 _edgeMovementVelocity = Vector3.zero;
    private Vector3 _lastMousePosition;

    private void Start()
    {
        _screenWidth = Screen.width;
        _screenHeight = Screen.height;
    }
    
    private void Update()
    {
        HandleRotation();
        HandleZoom();
        HandleMouseMovement();
        HandleMovement();
        HandleEdgeMovement();
        
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

    private void HandleMouseMovement()
    {
        if (Input.GetMouseButtonDown(2))
        {
            _lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 positionDifference = Input.mousePosition - _lastMousePosition;
            Vector3 moveRight = transform.right * (-positionDifference.x) * mouseMovementSpeed * Time.deltaTime;
            Vector3 moveForward = transform.forward * (-positionDifference.y) * mouseMovementSpeed * Time.deltaTime;

            moveRight.y = 0;
            moveForward.y = 0;
            
            Vector3 movement = moveRight + moveForward;
            Vector3 targetPosition = transform.position + movement;
            
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _mouseMovementVelocity, _smoothTime);
            _lastMousePosition = Input.mousePosition;
        }
    }

    private void HandleEdgeMovement()
    {
        Vector3 targetPosition = transform.position;
        Vector3 mousePosition = Input.mousePosition;
        Vector3 flatForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;

        if (mousePosition.x > _screenWidth - edgeTreshold)
        {
            targetPosition += transform.right * edgeMovementSpeed * Time.deltaTime;
        }

        if (mousePosition.x < edgeTreshold)
        {
            targetPosition -= transform.right * edgeMovementSpeed * Time.deltaTime;
        }

        if (mousePosition.y > _screenHeight - edgeTreshold)
        {
            targetPosition += flatForward * edgeMovementSpeed * Time.deltaTime;
        }

        if (mousePosition.y < edgeTreshold)
        {
            targetPosition -= flatForward * edgeMovementSpeed * Time.deltaTime;
        }
        
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _edgeMovementVelocity, _smoothTime);
    }
}
