using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : Actor
{
    [Header ("References")]
    public NavMeshSurface Surface;
    public Transform SpawnBullet;
    
    private AudioSource bulletSound;
    private NavMeshAgent agent;
    private GravityInverter gravity;
    private AsyncOperation navMeshOperation;
    private Animator enemyAnim;
    private RigBuilder rigbuilder;
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

        rigbuilder = gameObject.GetComponent<RigBuilder>();

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
        var bounds = new Bounds(Surface.transform.position, Vector3.one * 500f);

        NavMeshBuilder.CollectSources(bounds, Surface.layerMask, Surface.useGeometry, Surface.defaultArea, new List<NavMeshBuildMarkup>(), sources);
        navMeshOperation = NavMeshBuilder.UpdateNavMeshDataAsync(data, Surface.GetBuildSettings(), sources, bounds);

        yield return navMeshOperation;

        if (navMeshOperation.isDone)
        {
            Surface.navMeshData = data;
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

    protected override void Death()
    {
        rigbuilder.layers[0].active = false;

        enemyAnim.SetTrigger("isDead");
        isShooting = false;
    }

    // METHOD CALLED BY DEATH ANIMATION EVENT
    public void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}