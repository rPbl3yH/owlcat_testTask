using UnityEngine;

public sealed class CameraController : MonoBehaviour
{
    [SerializeField] private MeshGeometryDatabase _geometryDatabase;

    [SerializeField] private LayerMask _meshLayerMask;

    [SerializeField] private float _movementSpeed = 12f;
    [SerializeField] private float _movementDamping = 10f;
    [SerializeField] private float _zoomStep = 2f;
    [SerializeField] private float _minimumHeightOffset = 6f;
    [SerializeField] private float _maximumHeightOffset = 30f;
    [SerializeField] private float _initialHeightOffset = 12f;
    [SerializeField] private float _currentHeightOffset;

    private void Awake()
    {
        _currentHeightOffset = Mathf.Clamp(_initialHeightOffset, _minimumHeightOffset, _maximumHeightOffset);

        if (_geometryDatabase == null || _geometryDatabase.Entries == null)
        {
            Debug.LogError("Mesh geometry database not found. Using default camera behavior.");
        }
    }

    private void Update()
    {
        Vector3 nextPosition = CalculateNextPosition();
        float groundHeight = CalculateGroundHeight(nextPosition);
        Vector3 targetPosition = new Vector3(nextPosition.x, groundHeight + _currentHeightOffset, nextPosition.z);

        var lerpValue = 1f - Mathf.Exp(-_movementDamping * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpValue);

        HandleZoom();
    }

    private float CalculateGroundHeight(Vector3 horizontalPos)
    {
        float bestHeight = float.MinValue;
        bool isFoundHeight = false;

        foreach (MeshGeometryEntry entry in _geometryDatabase.Entries)
        {
            if (!MeshIntersection.RaycastDown(entry, horizontalPos, out float meshHeight)) 
                continue;

            if (isFoundHeight && meshHeight <= bestHeight) 
                continue;
            
            bestHeight = meshHeight;
            isFoundHeight = true;
        }

        Ray ray = new Ray(new Vector3(horizontalPos.x, 1000f, horizontalPos.z), Vector3.down);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 2000f, _meshLayerMask))
        {
            float hitY = hit.point.y;
            if (!isFoundHeight || hitY > bestHeight)
            {
                bestHeight = hitY;
                isFoundHeight = true;
            }
        }

        if (!isFoundHeight)
        {
            return transform.position.y - _currentHeightOffset;
        }

        return bestHeight;
    }

    private Vector3 CalculateNextPosition()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 direction = forward * v + right * h;
        direction.Normalize();

        return transform.position + direction * _movementSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        float wheel = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(wheel) <= Mathf.Epsilon)
            return;
        
        var zoomOffset = _currentHeightOffset - wheel * _zoomStep * 10f;
        _currentHeightOffset = Mathf.Clamp(zoomOffset, _minimumHeightOffset, _maximumHeightOffset);
    }
}