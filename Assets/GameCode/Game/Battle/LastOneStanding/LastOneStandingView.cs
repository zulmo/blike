using UnityEngine;

public class LastOneStandingView : MonoBehaviour
{
    [SerializeField]
    private PlayerScoresTable _scoresTable;

    [SerializeField]
    private BattleResultScreen _resultScreen;

    public void Initialize()
    {
        _scoresTable.Initialize();
        _resultScreen.Initialize();
        _resultScreen.gameObject.SetActive(false);
    }

    public void Deinitialize()
    {
        _scoresTable.Deinitialize();
        _resultScreen.Deinitialize();
    }
}
