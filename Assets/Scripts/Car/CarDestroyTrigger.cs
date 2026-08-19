using UnityEngine;

public class CarDestroyTrigger : MonoBehaviour
{
    [SerializeField] private CarController car;
    [SerializeField] private LayerMask destroyLayerMask;
    [SerializeField, Range(-1f, 1f)] private float flippedUpThreshold = 0f;

    private void OnCollisionEnter(Collision collision)
    {
        if ((destroyLayerMask.value & (1 << collision.gameObject.layer)) != 0 && IsFlipped())
        {
            car.Kill();
        }
    }

    private bool IsFlipped()
    {
        return Vector3.Dot(car.Body.transform.up, Vector3.up) < flippedUpThreshold;
    }
}
