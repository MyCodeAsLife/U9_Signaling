using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Sensor : MonoBehaviour
{
    public event Action OnIntruderEnter;
    public event Action OnIntruderExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Intruder>(out Intruder robber))
            OnIntruderEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Intruder>(out Intruder robber))
            OnIntruderExit?.Invoke();
    }
}
