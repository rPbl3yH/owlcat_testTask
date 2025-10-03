using UnityEngine;

public sealed class PlayerMoveController : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _raycastMaxDistance = 2000f;

    private void Update()
    {
        HandleClick();
    }

    private void HandleClick()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, _raycastMaxDistance, _groundLayerMask))
        {
            _character.SetDestination(hitInfo.point);
        }
    }
}