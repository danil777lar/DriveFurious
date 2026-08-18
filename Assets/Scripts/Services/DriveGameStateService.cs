using Larje.Core;
using Larje.Core.Services;
using Larje.Core.Services.UI;
using ProjectConstants;
using UnityEngine;
using UnityEngine.InputSystem;

[BindService(typeof(DriveGameStateService), typeof(IGameStateService))]
public class DriveGameStateService : GameStateService
{
    [InjectService] private UIService _uiService;

    public override void Init()
    {
        StartGame();
    }

    public void StartGame()
    {
        SetGameState(GameStates.Playing);
        _uiService.GetProcessor<UIScreenProcessor>().OpenScreen(new PlayScreen.Args());
    }

    public void WinGame()
    {
        SetGameState(GameStates.Win);
        _uiService.GetProcessor<UIScreenProcessor>().OpenScreen(new WinScreen.Args(StartGame));
    }

    public void FailGame()
    {
        SetGameState(GameStates.Fail);
        _uiService.GetProcessor<UIScreenProcessor>().OpenScreen(new FailScreen.Args(StartGame));
    }
}
