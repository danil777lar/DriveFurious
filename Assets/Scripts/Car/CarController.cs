using System;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private float maxDriveTorque = 2000f;
    [SerializeField] private float throttleRampSpeed = 2f;

    private float _targetThrottle;
    private float _currentThrottle;
    private readonly List<Func<bool>> _wheelGroundedStates = new List<Func<bool>>();

    public Rigidbody Body => body;
    public float WheelTorque { get; private set; }

    public event Action EventKilled;

    public void Kill()
    {
        SetThrottle(0f);
        EventKilled?.Invoke();
    }

    public void RegisterWheel(Func<bool> isGrounded)
    {
        _wheelGroundedStates.Add(isGrounded);
    }

    public float GetGroundedRatio()
    {
        if (_wheelGroundedStates.Count == 0)
        {
            return 0f;
        }

        int groundedCount = 0;
        for (int i = 0; i < _wheelGroundedStates.Count; i++)
        {
            if (_wheelGroundedStates[i].Invoke())
            {
                groundedCount++;
            }
        }

        return (float) groundedCount / _wheelGroundedStates.Count;
    }

    public void SetThrottle(float throttle)
    {
        _targetThrottle = Mathf.Clamp(throttle, -1f, 1f);
    }

    private void FixedUpdate()
    {
        _currentThrottle = Mathf.MoveTowards(_currentThrottle, _targetThrottle, throttleRampSpeed * Time.fixedDeltaTime);
        WheelTorque = _currentThrottle * maxDriveTorque;
    }
}
