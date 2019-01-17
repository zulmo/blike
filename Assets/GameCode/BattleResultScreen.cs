using UnityEngine;
using UnityEngine.UI;

public class BattleResultScreen : MonoBehaviour
{
    private static string WinTextFormat = "PLAYER {0} WINS!";

    [Header("Win Layout")]
    [SerializeField]
    private GameObject _winRoot;

    [SerializeField]
    private Image _winnerColor;

    [SerializeField]
    private Text _winText;

    [Header("Draw Layout")]
    [SerializeField]
    private GameObject _drawRoot;

    public void Initialize()
    {
        GameFacade.GameOver.Connect(OnGameOver);
    }

    public void Deinitialize()
    {
        GameFacade.GameOver.Disconnect(OnGameOver);
    }

    public void OnGameOver(int winnerIndex)
    {
        bool hasWinner = winnerIndex >= 0;

        SetWinLayout(hasWinner);

        if (hasWinner)
        {
            var playerModel = ApplicationModels.GetModel<GameModel>().Players[winnerIndex];
            _winnerColor.color = playerModel.Color;
            _winText.text = string.Format(WinTextFormat, winnerIndex + 1);
        }

        gameObject.SetActive(true);
    }

    private void SetWinLayout(bool isWin)
    {
        _winRoot.SetActive(isWin);
        _drawRoot.SetActive(!isWin);
    }
}
