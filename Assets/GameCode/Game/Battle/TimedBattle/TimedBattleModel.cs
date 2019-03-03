using System.Collections.Generic;
using UnityEngine;

public class TimedBattleModel : IGameMode
{
    // TODO: Set this from the menu
    private int _timeLimit = 60; // In seconds

    private List<PlayerBattleModel> _playersToRespawn = new List<PlayerBattleModel>();

    public float ElapsedTime { get; private set; }
    
    private bool IsOver
    {
        get
        {
            return ElapsedTime >= _timeLimit;
        }
    }

    #region IGameMode
    public void OnUpdate()
    {
        if (!IsOver)
        {
            ElapsedTime += Time.deltaTime;
            var secondsRemaining = Mathf.CeilToInt(_timeLimit - ElapsedTime);
            GameFacade.TimeRemainingChanged.Invoke(secondsRemaining);

            if (IsOver)
            {
                var players = ApplicationModels.GetModel<BattleModel>().Players;
                var winners = new List<int>();
                var bestScore = int.MinValue;
                for(int i = 0, count = players.Count; i < count; ++i)
                {
                    var playerScore = players[i].Score;
                    if(playerScore >= bestScore)
                    {
                        if(playerScore > bestScore)
                        {
                            winners.Clear();
                        }

                        winners.Add(i);
                        bestScore = playerScore;
                    }
                }

                var winnerIndex = winners.Count > 1 ? -1 : winners[0];
                GameFacade.BattleEnd.Invoke(winnerIndex, true);
            }
            else
            {
                // TODO: wait for a few seconds before respawn
                for (int i = 0, count = _playersToRespawn.Count; i < count; ++i)
                {
                    GameFacade.SpawnPlayer.Invoke(_playersToRespawn[i]);
                }

                _playersToRespawn.Clear();
            }
        }
    }

    public void OnPlayerHit(PlayerBattleModel victim, PlayerBattleModel attacker)
    {
        attacker.Score += victim == attacker ? -1 : 1;
        _playersToRespawn.Add(victim);
    }

    public void OnBombExploded()
    {
    }
    #endregion
}
