using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimationComponent : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private readonly int _speedParameter = Animator.StringToHash("Speed");
    private NavMeshAgent _navMeshAgent;

    public void Construct(NavMeshAgent navMeshAgent)
    {
        _navMeshAgent = navMeshAgent;
    }

    public void OnUpdate(float deltaTime)
    {
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (_animator == null || _navMeshAgent == null)
        {
            return;
        }

        float speed = _navMeshAgent.velocity.magnitude;
        _animator.SetFloat(_speedParameter, speed);
    }
}