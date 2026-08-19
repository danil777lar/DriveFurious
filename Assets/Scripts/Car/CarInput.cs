using Larje.Core;
using UnityEngine;

public class CarInput : MonoBehaviour
{
    [SerializeField] private CarController car;

    [InjectService] private InputService _inputService;

    private void Start()
    {
        DIContainer.InjectTo(this);
    }

    private void Update()
    {
        car.SetThrottle(_inputService.PlayerMovement.x);
    }
}
