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

    [Header ("References")]
    public Transform PlayerRef;
    public Transform PlayerHitTarget;
    public Transform RaycastOrigin;

    [Header("AI Settings")]
    public float StoppingDistance = 2f;
    public float DetectionRange = 10f;
    public float AttackDelay = 1f;

    private State currentState;
    private NavMeshAgent agent;
    private EnemyAI enemyScript;
    private Animator enemyAnim;
    private bool isAttacking = false, playerVisible = false;
    private int attackCount = 0;

    private void Start()
    {
        enemyScript = GetComponent<EnemyAI>();
        agent = GetComponent<NavMeshAgent>();
        enemyAnim = GetComponent<Animator>();

        agent.stoppingDistance = StoppingDistance;
        currentState = State.Idle;
    }

    private void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, PlayerRef.position);
        Vector3 dir = PlayerHitTarget.position - RaycastOrigin.position;
        RaycastHit hit;

        if (playerDistance <= DetectionRange)
        {
            Physics.Raycast(RaycastOrigin.position, dir, out hit, Mathf.Infinity);

            if (hit.transform.gameObject.CompareTag("Surface"))
            {
                Debug.DrawRay(RaycastOrigin.position, dir * 1000, Color.red);
                playerVisible = false;
            }
            else
            {
                Debug.DrawRay(RaycastOrigin.position, dir * 1000, Color.green);
                playerVisible = true;
            }
        }

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
            agent.SetDestination(PlayerRef.position);
            enemyAnim.SetFloat("VelocityZ", agent.speed);
        }
    }

    private void AttackPlayer()
    {
        if (!isAttacking)
        {
            attackCount++;
            
            if (attackCount == 4)
            {
                AttackDelay = 4f;
                attackCount = 0;
            }else if (attackCount < 4)
            {
                AttackDelay = 1f;
            }
            
            enemyScript.Attack();
            enemyAnim.SetFloat("VelocityZ", 0);
            StartCoroutine(AttackDelayCount());
        }
    }

    private IEnumerator AttackDelayCount()
    {
        isAttacking = true;
        if (AttackDelay == 4f)
        {
            enemyAnim.SetBool("IsFiring", false);
        }
        yield return new WaitForSeconds(AttackDelay);
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Surface") && currentState == State.Chasing)
        {
            currentState = State.Idle;
            agent.isStopped = true;
            enemyAnim.SetFloat("VelocityZ", 0);
        }
    }
}