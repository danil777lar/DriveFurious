using Larje.Core.Services;
using UnityEngine;

public class DriveLevel : LevelProcessor
{
    private LevelData _levelData;

    public override void TryStartLevel(StartData data)
    {
        StartLevel(data);
    }

    public override void TryStopLevel(StopData data)
    {
        StopLevel(data);
    }

    public override LevelProcessor.LevelData GetLevelData()
    {
        return _levelData;
    }

    private void Start()
    {
        _levelData = new LevelData(this);
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
