using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform[] points;
    public int currentPoint;
    Vector3 target;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentPoint = 0;
        UpdateDestination();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDestination();
        if (Vector3.Distance(transform.position, target) < 1)
        {
            IteratePointsIndex();
        }
    }
    public void UpdateDestination()
    {
        target = points[currentPoint].position;
        agent.SetDestination(target);
    }
    void IteratePointsIndex()
    {
        currentPoint++;
        if (currentPoint == points.Length)
        {
            currentPoint = 0;
        }
    }
}
