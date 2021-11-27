using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform[] points;
    int currentPoint;
    Vector3 target;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        UpdateDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, target) < 2)
        {
            IteratePointsIndex();
            UpdateDestination();
        }
    }
    void UpdateDestination()
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
