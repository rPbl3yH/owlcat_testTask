using UnityEngine;
using UnityEngine.AI;

public class CharacterRotateComponent : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 12f;
    [SerializeField] private Transform _visualTransform;

    private NavMeshAgent _navMeshAgent;

    public void Construct(NavMeshAgent navMeshAgent)
    {
        _navMeshAgent = navMeshAgent;
        _navMeshAgent.updateRotation = false;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_navMeshAgent == null)
        {
            return;
        }

        Vector3 velocity = _navMeshAgent.velocity;
        velocity.y = 0f;

        if (velocity.sqrMagnitude <= Constants.EPSILON)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(velocity.normalized, Vector3.up);
        _visualTransform.rotation =
            Quaternion.Lerp(_visualTransform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }
}