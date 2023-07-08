using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : Actor
{
    public NavMeshSurface surface;
    public Transform SpawnBullet;
    [SerializeField] private AudioSource bulletSound;

    private NavMeshAgent agent;
    private GravityInverter gravity;
    private AsyncOperation navMeshOperation;
    
    private Animator enemyAnim;
    private bool isShooting;

    protected override void Awake()
    {
        base.Awake();
        
        bulletSound = GetComponent<AudioSource>();

        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;

        gravity = GetComponent<GravityInverter>();

        StartCoroutine(BuildNavMesh());
        
        enemyAnim = GetComponent<Animator>();
        
        energy = 100f;
        isShooting = true;
    }

    void Update()
    {
        if (gravity.ShouldRotate && agent.enabled)
        {
            agent.enabled = false;
        }

        if (!gravity.ShouldRotate && !agent.enabled)
        {
            agent.enabled = true;
            StartCoroutine(BuildNavMesh());
        }
    }

    IEnumerator BuildNavMesh()
    {
        var data = new NavMeshData();
        var sources = new List<NavMeshBuildSource>();
        var bounds = new Bounds(surface.transform.position, Vector3.one * 500f);

        NavMeshBuilder.CollectSources(bounds, surface.layerMask, surface.useGeometry, surface.defaultArea, new List<NavMeshBuildMarkup>(), sources);
        navMeshOperation = NavMeshBuilder.UpdateNavMeshDataAsync(data, surface.GetBuildSettings(), sources, bounds);

        yield return navMeshOperation;

        if (navMeshOperation.isDone)
        {
            surface.navMeshData = data;
            agent.enabled = true;
        }
    }

    public void Attack()
    {
        if (isShooting)
        {
            base.Shoot(SpawnBullet);
            bulletSound.Play();
        }
    }

    protected override void EnemyDeath()
    {
        base.EnemyDeath();
    
        enemyAnim.SetTrigger("isDead");
        isShooting = false;
    }

    public void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }

}