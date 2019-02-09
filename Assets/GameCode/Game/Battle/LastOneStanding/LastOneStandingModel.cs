using System.Collections.Generic;

public class LastOneStandingModel : IGameMode
{
    public void CheckRoundOverCondition(PlayerController player, Bomb bomb)
    {
        throw new System.NotImplementedException();
    }

    bool IGameMode.IsRoundOver(List<PlayerController> players, out int winnerIndex)
    {
        winnerIndex = int.MinValue;

        var activePlayers = players.FindAll(controller => controller.gameObject.activeInHierarchy);
        var nbActivePlayers = activePlayers.Count;
        bool isOver = nbActivePlayers <= 1;
        if (isOver)
        {
            winnerIndex = nbActivePlayers == 0 ? -1 : players.FindIndex(o => o == activePlayers[0]);
        }

        return isOver;
    }

    void IGameMode.OnPlayerHit(PlayerBattleModel victim)
    {
        victim.Events.Eliminated.Invoke();
    }
}
