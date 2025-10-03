using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private CharacterAnimationComponent _animationComponent;
    [SerializeField] private CharacterRotateComponent _rotateComponent;

    private void Awake()
    {
        _animationComponent.Construct(_navMeshAgent);
        _rotateComponent.Construct(_navMeshAgent);
    }

    private void Update()
    {
        _animationComponent.OnUpdate(Time.deltaTime);
        _rotateComponent.OnUpdate(Time.deltaTime);
    }

    public void SetDestination(Vector3 hitInfoPoint)
    {
        _navMeshAgent.SetDestination(hitInfoPoint);
    }
}