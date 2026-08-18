using System;
using System.Collections.Generic;
using Larje.Core;
using ProjectConstants;
using UnityEngine;
using UnityEngine.InputSystem;

using PlayerActions = InputSystem_Actions.PlayerActions;
using UIActions = InputSystem_Actions.UIActions;

[BindService(typeof(PlayerInputService), typeof(InputService))]
public class PlayerInputService : InputService
{
    public override Vector2 PlayerMovement => GetActions<PlayerActions>().Move.ReadValue<Vector2>();
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
}
