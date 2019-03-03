using System;
using UnityEngine;

public class TimedBattleView : FreeForAllGameView
{
    [SerializeField]
    private Timer _timer;

    #region GameView abstract methods
    public override void Initialize()
    {
        base.Initialize();
        GameFacade.TimeRemainingChanged.Connect(OnTimeRemainingChanged);
    }

    public override void Deinitialize()
    {
        base.Deinitialize();
        GameFacade.TimeRemainingChanged.Disconnect(OnTimeRemainingChanged);
    }
    #endregion

    private void OnTimeRemainingChanged(int secondsRemaining)
    {
        _timer.UpdateTime(secondsRemaining);
    }
}
