using Larje.Core;
using Larje.Core.Services;
using ProjectConstants;
using UnityEngine;

public class DriveLevel : LevelProcessor
{
    [SerializeField] private CarController[] cars;
    [SerializeField] private FinishTrigger[] finishTriggers;

    [InjectService] private IGameStateService _gameStateService;

    private LevelData _levelData;

    public override void TryStartLevel(StartData data)
    {
        StartLevel(data);
    }

    public override void TryStopLevel(StopData data)
    {
        StopLevel(data);
        if (data.StopType == LevelStopType.Win)
        {
            _gameStateService.SetGameState(GameStates.Win);
        }
        else if (data.StopType == LevelStopType.Fail)
        {
            _gameStateService.SetGameState(GameStates.Fail);
        }
    }

    public override LevelProcessor.LevelData GetLevelData()
    {
        return _levelData;
    }

    private void Start()
    {
        DIContainer.InjectTo(this);

        _levelData = new LevelData(this);

        foreach (CarController car in cars)
        {
            car.EventKilled += OnCarKilled;
        }

        foreach (FinishTrigger finishTrigger in finishTriggers)
        {
            finishTrigger.EventFinished += OnFinishTriggered;
        }
    }

    private void OnDestroy()
    {
        foreach (CarController car in cars)
        {
            car.EventKilled -= OnCarKilled;
        }

        foreach (FinishTrigger finishTrigger in finishTriggers)
        {
            finishTrigger.EventFinished -= OnFinishTriggered;
        }
    }

    private void OnCarKilled()
    {
        TryStopLevel(new StopData(false, LevelStopType.Fail));
    }

    private void OnFinishTriggered()
    {
        TryStopLevel(new StopData(true, LevelStopType.Win));
    }

    public new class LevelData : LevelProcessor.LevelData
    {
        private readonly DriveLevel _level;

        public LevelData(DriveLevel level)
        {
            _level = level;
        }
    }
}
