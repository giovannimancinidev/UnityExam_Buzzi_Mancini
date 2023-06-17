using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : Actor
{
    public NavMeshSurface surface;
    public Transform SpawnBullet;

    private NavMeshAgent agent;
    private GravityInverter gravity;
    private AsyncOperation navMeshOperation;

    protected override void Awake()
    {
        base.Awake();

        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;

        gravity = GetComponent<GravityInverter>();

        StartCoroutine(BuildNavMesh());
        
        energy = 100f;
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
        
        //DA MODIFICARE
        if (energy <= 0)
        {
            gameObject.SetActive(false);
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
        base.Shoot(SpawnBullet);
    }
}