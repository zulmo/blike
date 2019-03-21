using UnityEngine;
using UnityEngine.UI;

public class GameEndRankingItem : MonoBehaviour
{
    private static string PlayerNameFormat = "Player {0}";

    [SerializeField]
    private Image _playerColor;

    [SerializeField]
    private Text _playerName;

    [SerializeField]
    private Text _playerScore;

    [SerializeField]
    private PlayerReadyStatusWidget _readyStatus;

    [SerializeField]
    private Image _greyFilter;

    public void Setup(PlayerBattleModel player)
    {
        _playerColor.color = player.PlayerModel.Color;
        _playerScore.text = player.Score.ToString();

        var playerIndex = ApplicationModels.GetModel<BattleModel>().Players.FindIndex(player);
        _playerName.text = string.Format(PlayerNameFormat, playerIndex + 1);

        SetPlayerReady(false);
    }

    public void SetPlayerReady(bool ready)
    {
        _readyStatus.SetReady(ready);
        _greyFilter.gameObject.SetActive(ready);
    }
}
