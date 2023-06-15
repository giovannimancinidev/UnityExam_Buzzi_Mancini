using UnityEngine;
using UnityEngine.AI;

public class EnemyAIStateMachine : MonoBehaviour
{
    public enum State
    {
        Idle,
        Chasing,
        Attacking
    }

    public State currentState;
    public Transform player;
    private NavMeshAgent agent;
    public float stoppingDistance = 2f;
    public float detectionRange = 10f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;
        currentState = State.Idle;
    }

    private void Update()
    {
        float playerDistance = Vector3.Distance(new Vector3(transform.position.x, player.position.y, player.position.z), transform.position);

        switch (currentState)
        {
            case State.Idle:
                if (playerDistance <= detectionRange)
                {
                    currentState = State.Chasing;
                }
                break;

            case State.Chasing:
                if (Mathf.Abs(transform.position.x - player.position.x) <= 1f)
                {
                    currentState = State.Attacking;
                    if (agent.enabled)
                    {
                        agent.isStopped = true;
                    }
                }
                else
                {
                    if (agent.enabled && currentState != State.Attacking)
                    {
                        agent.isStopped = false;
                        ChasePlayer();
                    }
                }
                break;

            case State.Attacking:
                if (Mathf.Abs(transform.position.x - player.position.x) > 1f || playerDistance > detectionRange)
                {
                    currentState = State.Chasing;
                    if (agent.enabled)
                    {
                        agent.isStopped = false;
                    }
                }
                break;
        }
    }

    private void ChasePlayer()
    {
        if (agent.enabled)
        {
            Vector3 targetPosition = new Vector3(transform.localPosition.x, player.localPosition.y, player.localPosition.z);
            
            agent.SetDestination(targetPosition);
        }
    }

    private void AttackPlayer()
    {
        // Attack logic here
    }
}