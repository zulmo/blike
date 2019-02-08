using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoresTable : MonoBehaviour
{
    [SerializeField]
    private VerticalLayoutGroup _leftColumn;

    [SerializeField]
    private VerticalLayoutGroup _rightColumn;

    [SerializeField]
    private PlayerScoreUI _scoreUiPrefab;

    private List<PlayerScoreUI> _scoreItems;

    public void Initialize()
    {
        _scoreItems = new List<PlayerScoreUI>();
        var players = ApplicationModels.GetModel<BattleModel>().Players;
        for(int i = 0, count = players.Count; i < count; ++i)
        {
            var parent = i % 2 == 0 ? _leftColumn : _rightColumn;
            var item = Instantiate(_scoreUiPrefab, parent.transform);
            item.Initialize(players[i]);
            _scoreItems.Add(item);
        }
    }

    public void Deinitialize()
    {
        for (int i = 0, count = _scoreItems.Count; i < count; ++i)
        {
            _scoreItems[i].Deinitialize();
        }
        _scoreItems.Clear();
    }
}
