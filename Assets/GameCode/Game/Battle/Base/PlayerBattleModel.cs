public class PlayerBattleModel
{
    public PlayerModel PlayerModel { get; private set; }
    public bool IsSpawned { get; set; }

    public PlayerBattleFacade Events { get; private set; }

    private int _score;
    public int Score
    {
        get
        {
            return _score;
        }

        set
        {
            _score = value;
            Events.ScoreChanged.Invoke(_score);
        }
    }

    public static int CompareScores(PlayerBattleModel player, PlayerBattleModel otherPlayer)
    {
        return otherPlayer.Score - player.Score;
    }

    public PlayerBattleModel(PlayerModel model)
    {
        PlayerModel = model;
        Events = new PlayerBattleFacade();
        Score = 0;
    }
}
