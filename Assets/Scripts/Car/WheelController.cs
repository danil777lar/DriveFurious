using UnityEngine;

public class WheelController : MonoBehaviour
{
    [SerializeField] private CarController car;
    [SerializeField] private Rigidbody wheelBody;
    [SerializeField] private Transform anchor;
    [SerializeField] private WheelConfig config;
    [Space]
    [SerializeField] private bool isDriven = true;
    [SerializeField] private Vector3 spinAxis = Vector3.right;

    private int _groundContactCount;

    public bool IsGrounded => _groundContactCount > 0;

    private void Start()
    {
        wheelBody.angularDamping = config.RollingResistance;
        SnapToRestPosition();
        car.RegisterWheel(() => IsGrounded);
    }

    private void FixedUpdate()
    {
        ApplyDriveTorque();
        ApplyMountForces();
    }

    private void SnapToRestPosition()
    {
        Vector3 restPosition = anchor.position - car.Body.transform.up * config.SuspensionRestLength;
        wheelBody.position = restPosition;
        wheelBody.linearVelocity = Vector3.zero;
        wheelBody.angularVelocity = Vector3.zero;
    }

    private void ApplyDriveTorque()
    {
        if (!isDriven)
        {
            return;
        }

        Vector3 torque = wheelBody.transform.TransformDirection(spinAxis) * car.WheelTorque;
        wheelBody.AddTorque(torque);

        float reactionScale = config.ReactionTorqueByGroundedRatio.Evaluate(car.GetGroundedRatio());
        car.Body.AddTorque(-torque * config.ReactionTorqueMultiplier * reactionScale);
    }

    private void ApplyMountForces()
    {
        Rigidbody body = car.Body;
        Transform bodyTransform = body.transform;

        Vector3 up = bodyTransform.up;
        Vector3 drive = bodyTransform.right;

        Vector3 anchorPosition = anchor.position;
        Vector3 anchorVelocity = body.GetPointVelocity(anchorPosition);

        Vector3 offset = wheelBody.position - anchorPosition;
        float driveOffset = Vector3.Dot(offset, drive);

        if (Mathf.Abs(driveOffset) > config.MaxDriveTravel)
        {
            float clampedDriveOffset = Mathf.Sign(driveOffset) * config.MaxDriveTravel;
            wheelBody.position += drive * (clampedDriveOffset - driveOffset);
            offset = wheelBody.position - anchorPosition;
        }

        float verticalOffset = Vector3.Dot(offset, up) + config.SuspensionRestLength;

        if (Mathf.Abs(verticalOffset) > config.MaxSuspensionTravel)
        {
            float clampedVerticalOffset = Mathf.Sign(verticalOffset) * config.MaxSuspensionTravel;
            wheelBody.position += up * (clampedVerticalOffset - verticalOffset);
            offset = wheelBody.position - anchorPosition;
            verticalOffset = clampedVerticalOffset;
        }

        Vector3 relativeVelocity = wheelBody.linearVelocity - anchorVelocity;
        float verticalVelocity = Vector3.Dot(relativeVelocity, up);
        float verticalForce = -config.SuspensionStiffness * verticalOffset - config.SuspensionDamping * verticalVelocity;

        float driveVelocity = Vector3.Dot(relativeVelocity, drive);
        float driveForce = -config.TractionStiffness * driveOffset - config.TractionDamping * driveVelocity;

        Vector3 forceOnWheel = up * verticalForce + drive * driveForce;
        forceOnWheel = Vector3.ClampMagnitude(forceOnWheel, config.MaxMountForce);

        wheelBody.AddForce(forceOnWheel);

        if (IsGrounded)
        {
            body.AddForceAtPosition(-forceOnWheel, anchorPosition);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody == car.Body)
        {
            return;
        }

        _groundContactCount++;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.rigidbody == car.Body)
        {
            return;
        }

        _groundContactCount--;
    }

    private void OnDrawGizmos()
    {
        if (wheelBody == null)
        {
            return;
        }

        Vector3 wheelPosition = wheelBody.position;

        Gizmos.color = IsGrounded ? (isDriven ? Color.cyan : Color.gray) : Color.white;
        Gizmos.DrawWireSphere(wheelPosition, 0.15f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(wheelPosition, wheelBody.transform.TransformDirection(spinAxis.normalized) * 0.3f);

        if (anchor == null)
        {
            return;
        }

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(anchor.position, 0.05f);

        Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
        Gizmos.DrawLine(anchor.position, wheelPosition);

        if (car != null && car.Body != null && config != null)
        {
            Vector3 restPosition = anchor.position - car.Body.transform.up * config.SuspensionRestLength;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(restPosition, 0.05f);
        }
    }
}
