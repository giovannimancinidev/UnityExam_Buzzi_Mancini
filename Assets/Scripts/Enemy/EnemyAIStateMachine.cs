using System.Collections;
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
    public float stoppingDistance = 2f;
    public float detectionRange = 10f;
    public float attackDelay = 1f;

    private NavMeshAgent agent;
    private EnemyAI enemyScript;
    private Animator enemyAnim;
    private bool isAttacking = false;

    private void Start()
    {
        enemyScript = GetComponent<EnemyAI>();
        agent = GetComponent<NavMeshAgent>();
        enemyAnim = GetComponent<Animator>();
        
        agent.stoppingDistance = stoppingDistance;
        currentState = State.Idle;
    }

    private void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);
        RaycastHit hit;
        bool playerVisible = Physics.Raycast(transform.position, transform.forward, out hit, detectionRange) && hit.transform == player;

        if (playerVisible)
        {
            if (currentState == State.Idle || currentState == State.Chasing)
            {
                agent.isStopped = true;
                currentState = State.Attacking;
            }
        }
        else if (currentState == State.Attacking)
        {
            currentState = State.Chasing;
        }

        switch (currentState)
        {
            case State.Idle:
                // Do nothing
                break;

            case State.Chasing:
                if (agent.enabled)
                {
                    agent.isStopped = false;
                    ChasePlayer();
                    enemyAnim.SetBool("IsFiring", false);
                }
                break;

            case State.Attacking:
                if (!isAttacking)
                {
                    AttackPlayer();
                    enemyAnim.SetBool("IsFiring", true);
                }
                break;
        }
    }

    private void ChasePlayer()
    {
        if (agent.enabled)
        {
            agent.SetDestination(player.position);
            enemyAnim.SetFloat("VelocityZ", agent.speed);
        }
    }

    private void AttackPlayer()
    {
        if (!isAttacking)
        {
            enemyScript.Attack();
            enemyAnim.SetFloat("VelocityZ", 0);
            StartCoroutine(AttackDelay());
        }
    }

    private IEnumerator AttackDelay()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }
}