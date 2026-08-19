using UnityEngine;

[CreateAssetMenu(menuName = "DriveFurious/Wheel Config", fileName = "Wheel Config")]
public class WheelConfig : ScriptableObject
{
    [field: Header("Drive")]
    [field: SerializeField] public float ReactionTorqueMultiplier { get; private set; } = 1f;

    [field: Header("Suspension")]
    [field: SerializeField] public float SuspensionRestLength { get; private set; } = 0.3f;
    [field: SerializeField] public float SuspensionStiffness { get; private set; } = 800f;
    [field: SerializeField] public float SuspensionDamping { get; private set; } = 60f;

    [field: Header("Traction")]
    [field: SerializeField] public float TractionStiffness { get; private set; } = 600f;
    [field: SerializeField] public float TractionDamping { get; private set; } = 40f;

    [field: Header("Safety")]
    [field: SerializeField] public float MaxMountForce { get; private set; } = 5000f;
}
