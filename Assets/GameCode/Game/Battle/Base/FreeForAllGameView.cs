using UnityEngine;

public class FreeForAllGameView : GameView
{
    [SerializeField]
    private PlayerScoresTable _scoresTable;

    [SerializeField]
    private BattleResultScreen _resultScreen;

    #region GameView abstract methods
    public override void Initialize()
    {
        _scoresTable.Initialize();
        _resultScreen.Initialize();
        _resultScreen.gameObject.SetActive(false);
    }

    public override void Deinitialize()
    {
        _scoresTable.Deinitialize();
        _resultScreen.Deinitialize();
    }
    #endregion
}
