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
        EventGameStateChanged += OnGameStateChanged;
        StartGame();
    }

    public void StartGame(LevelStartType startType = LevelStartType.Start)
    {
        _uiService.GetProcessor<UIScreenProcessor>().OpenScreen(new PlayScreen.Args());
        _levelManagerService.SpawnCurrentLevel(_ =>
        {
            _levelManagerService.TryStartCurrentLevel(new LevelProcessor.StartData(startType));
            SetGameState(GameStates.Playing);
        });
    }

    private void OnGameStateChanged(GameState previousState, GameState newState)
    {
        if (newState == GameStates.Win)
        {
            WinGame();
        }

        if (newState == GameStates.Fail)
        {
            FailGame();
        }
    }

    private void WinGame()
    {
        _levelManagerService.IncrementLevelId();
        _uiService.GetProcessor<UIScreenProcessor>().OpenScreen(new WinScreen.Args(() => StartGame(LevelStartType.Start)));
    }

    private void FailGame()
    {
        _uiService.GetProcessor<UIScreenProcessor>().OpenScreen(new FailScreen.Args(() => StartGame(LevelStartType.Restart)));
    }
}
