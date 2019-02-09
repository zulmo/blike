using System.Collections.Generic;

public interface IGameMode
{
    void OnPlayerHit(PlayerBattleModel victim);
    bool IsRoundOver(List<PlayerController> players, out int winnerIndex);
}