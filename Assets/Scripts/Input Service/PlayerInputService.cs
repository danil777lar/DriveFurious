using System;
using System.Collections.Generic;
using Larje.Core;
using Larje.Core.Services.UI;
using ProjectConstants;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

using PlayerActions = InputSystem_Actions.PlayerActions;
using UIActions = InputSystem_Actions.UIActions;

[BindService(typeof(PlayerInputService), typeof(InputService))]
public class PlayerInputService : InputService
{
    [SerializeField] private RectTransformEvents _pointerEvents;

    private float _touchInput;

    public override Vector2 PlayerMovement => GetPlayerMovement();
    public override InputAction UIBack => GetActions<UIActions>().Cancel;
    public override InputAction UIDebug => null;
    public override InputAction PlayerRun => GetActions<PlayerActions>().Sprint;
    public override InputAction PlayerPointer => GetActions<UIActions>().Point;

    public override Dictionary<InputActionMapType, Type> ActionMapTypes => new Dictionary<InputActionMapType, Type>
    {
        { InputActionMapType.Player, typeof(PlayerActions) },
        { InputActionMapType.UI, typeof(UIActions) },
    };

    public override Dictionary<Type, bool> DefaultStates => new Dictionary<Type, bool>
    {
        { typeof(PlayerActions), true },
        { typeof(UIActions), true },
    };

    private void Start()
    {
        _pointerEvents.EventPointerDown += OnPointerDown;
        _pointerEvents.EventPointerUp += OnPointerUp;
    }

    private void OnDestroy()
    {
        _pointerEvents.EventPointerDown -= OnPointerDown;
        _pointerEvents.EventPointerUp -= OnPointerUp;
    }

    private void OnPointerDown(PointerEventData eventData)
    {
        _touchInput = eventData.position.x < Screen.width / 2f ? -1f : 1f;
    }

    private void OnPointerUp(PointerEventData eventData)
    {
        _touchInput = 0f;
    }

    private Vector2 GetPlayerMovement()
    {
        Vector2 movement = GetActions<PlayerActions>().Move.ReadValue<Vector2>();
        movement.x += _touchInput;
        movement = Vector2.ClampMagnitude(movement, 1f);

        return movement;
    }
}
