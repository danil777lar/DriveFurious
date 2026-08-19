using System;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask triggerLayerMask;

    public event Action EventFinished;

    private void OnTriggerEnter(Collider other)
    {
        if ((triggerLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            EventFinished?.Invoke();
        }
    }
}
