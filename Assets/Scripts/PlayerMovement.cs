using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

public class DogMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        CamController.newDogDestination += SetDogDestination;
        agent = GetComponent<NavMeshAgent>();
    }
    private void OnDisable()
    {
        CamController.newDogDestination -= SetDogDestination;
    }

    private void SetDogDestination(Vector3 _newDes)
    {
        agent.SetDestination(_newDes);
    }
}
