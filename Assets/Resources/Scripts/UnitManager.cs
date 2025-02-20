using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    public NavMeshAgent agent;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {

    }
}
