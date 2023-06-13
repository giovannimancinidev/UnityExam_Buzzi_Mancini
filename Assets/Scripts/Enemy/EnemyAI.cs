using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private GravityInverter gravity;

    public NavMeshSurface surface;
    private AsyncOperation navMeshOperation;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        
        gravity = GetComponent<GravityInverter>();

        StartCoroutine(BuildNavMesh());
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
}