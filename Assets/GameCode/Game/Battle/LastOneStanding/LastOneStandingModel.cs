public class LastOneStandingModel : IGameMode
{
    public int NbRoundsToWin { get; private set; }

    public LastOneStandingModel()
    {
        NbRoundsToWin = 3;
    }

    #region IGameMode
    public void OnUpdate()
    {
    }

    public void OnBombExploded()
    {
        var players = ApplicationModels.GetModel<BattleModel>().Players;
        var activePlayers = players.FindAll(player => player.IsSpawned);
        var nbActivePlayers = activePlayers.Count;
        bool isRoundOver = nbActivePlayers <= 1;
        if (isRoundOver)
        {
            int winnerIndex = -1;
            bool isGameOver = false;

            if(nbActivePlayers > 0)
            {
                var winner = activePlayers[0];
                winnerIndex = players.FindIndex(winner);
                isGameOver = ++winner.Score == NbRoundsToWin;
            }
            GameFacade.BattleEnd.Invoke(winnerIndex, isGameOver);
        }
    }

    public void OnPlayerHit(PlayerBattleModel victim, PlayerBattleModel attacker)
    {
        victim.Events.Eliminated.Invoke();
    }
    #endregion
}
