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
    [InjectService] private ILevelManagerService _levelManagerService;

    public override void Init()
    {
        StartGame();
    }

    public void StartGame()
    {
        _uiService.GetProcessor<UIScreenProcessor>().OpenScreen(new PlayScreen.Args());
        _levelManagerService.SpawnCurrentLevel(_ =>
            _levelManagerService.TryStartCurrentLevel(new LevelProcessor.StartData(LevelStartType.Start)));
    }

    public void RestartGame()
    {
        _uiService.GetProcessor<UIScreenProcessor>().OpenScreen(new PlayScreen.Args());
        _levelManagerService.SpawnCurrentLevel(_ =>
            _levelManagerService.TryStartCurrentLevel(new LevelProcessor.StartData(LevelStartType.Restart)));
    }

    public void WinGame()
    {
        SetGameState(GameStates.Win);
        _uiService.GetProcessor<UIScreenProcessor>().OpenScreen(new WinScreen.Args(StartGame));
    }

    public void FailGame()
    {
        SetGameState(GameStates.Fail);
        _uiService.GetProcessor<UIScreenProcessor>().OpenScreen(new FailScreen.Args(RestartGame));
    }
}
