using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerCollisionEvents : MonoBehaviour
{
    public UnityEvent<Collider> onEnter;

    public UnityEvent<Collider> onExit;


    private void OnTriggerEnter(Collider other)
    {
        onEnter?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        onExit?.Invoke(other);
    }
}
