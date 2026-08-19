using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private float maxDriveTorque = 2000f;

    private float _throttle;

    public Rigidbody Body => body;
    public float WheelTorque { get; private set; }

    public void SetThrottle(float throttle)
    {
        _throttle = Mathf.Clamp(throttle, -1f, 1f);
    }

    private void FixedUpdate()
    {
        WheelTorque = _throttle * maxDriveTorque;
    }
}
