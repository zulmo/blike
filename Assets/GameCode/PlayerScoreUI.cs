using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreUI : MonoBehaviour
{
    [SerializeField]
    private Image _playerColor;
    
    [SerializeField]
    private Text _playerScore;

    [SerializeField]
    private Image _grayFilter;

    private PlayerBattleModel _battleModel;

    public void Initialize(PlayerBattleModel playerBattleModel)
    {
        if(_battleModel != null)
        {
            Deinitialize();
        }

        _battleModel = playerBattleModel;
        _battleModel.Events.ScoreChanged.Connect(SetScore);
        _battleModel.Events.Eliminated.Connect(OnEliminated);

        SetScore(_battleModel.Score);

        _playerColor.color = _battleModel.PlayerModel.Color;

        _grayFilter.gameObject.SetActive(false);
    }

    public void Deinitialize()
    {
        _battleModel.Events.ScoreChanged.Disconnect(SetScore);
    }

    private void SetScore(int score)
    {
        _playerScore.text = score.ToString();
    }

    private void OnEliminated()
    {
        _grayFilter.gameObject.SetActive(true);
    }

}
