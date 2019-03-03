public interface IGameMode
{
    void OnUpdate();
    void OnPlayerHit(PlayerBattleModel victim, PlayerBattleModel attacker);
    void OnBombExploded();
}