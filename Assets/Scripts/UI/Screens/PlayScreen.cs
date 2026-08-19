using Larje.Core;
using Larje.Core.Services.UI;
using ProjectConstants;
using UnityEngine;
using UnityEngine.UI;

public class PlayScreen : UIScreen
{
    [SerializeField] private Button restartButton;

    [InjectService] private DriveGameStateService _gameStateService;

    protected override void OnBeforeOpen(UIObject.Args args)
    {
        DIContainer.InjectTo(this);

        restartButton.onClick.AddListener(OnRestartButtonClicked);
    }

    protected override void OnBeforeClose()
    {
        restartButton.onClick.RemoveListener(OnRestartButtonClicked);
    }

    private void OnRestartButtonClicked()
    {
        _gameStateService.StartGame(LevelStartType.Restart);
    }

    public class Args : UIScreen.Args
    {
        public Args() : base(UIScreenType.Play)
        {
        }
    }
}
