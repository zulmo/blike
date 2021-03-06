﻿using UnityEngine;

public static class GameModeFactory
{
    public static IGameMode Create(EGameMode mode)
    {
        switch(mode)
        {
            case EGameMode.LastOneStanding: return new LastOneStandingModel();
            case EGameMode.TimedBattle: return new TimedBattleModel();
        }

        Debug.LogError(string.Format("Unhandled game mode: {0}", mode.ToString()));
        return null;
    }
}
