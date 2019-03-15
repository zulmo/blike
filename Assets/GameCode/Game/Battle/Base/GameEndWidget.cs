using UnityEngine;
using UnityEngine.UI;

public class GameEndWidget : MonoBehaviour
{
    private static string WinTextFormat = "PLAYER {0} WINS THE GAME!";

    [SerializeField]
    private Text _winText;

    [SerializeField]
    private GameEndRankings _rankings;

    public void Show(int winnerIndex)
    {
        _winText.text = winnerIndex < 0 ? "DRAW!" : string.Format(WinTextFormat, winnerIndex + 1);
        gameObject.SetActive(true);

        _rankings.ShowRankings();
    }
}
