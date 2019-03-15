using System.Collections.Generic;
using UnityEngine;

public class GameEndRankings : MonoBehaviour
{
    [SerializeField]
    private GameEndRankingItem _rankingItemPrefab;

    public void ShowRankings()
    {
        var players = new List<PlayerBattleModel>(ApplicationModels.GetModel<BattleModel>().Players);
        players.Sort(PlayerBattleModel.CompareScores);
        players.ForEach(AddRankingItem);
    }

    private void AddRankingItem(PlayerBattleModel player)
    {
        var item = Instantiate(_rankingItemPrefab, transform);
        item.Setup(player);
    }
}
