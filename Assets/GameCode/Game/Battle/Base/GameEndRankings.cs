using System.Collections.Generic;
using UnityEngine;

public class GameEndRankings : MonoBehaviour
{
    [SerializeField]
    private GameEndRankingItem _rankingItemPrefab;

    private struct ReadyStatusPair
    {
        internal GameEndRankingItem Widget;
        internal bool IsReady;
    }

    private Dictionary<PlayerModel, ReadyStatusPair> _rankingItems = new Dictionary<PlayerModel, ReadyStatusPair>();

    public void ShowRankings()
    {
        var players = new List<PlayerBattleModel>(ApplicationModels.GetModel<BattleModel>().Players);
        players.Sort(PlayerBattleModel.CompareScores);
        players.ForEach(AddRankingItem);
        GameFacade.ReadyInputPressed.Connect(OnReadyInputPressed);
    }

    private void AddRankingItem(PlayerBattleModel player)
    {
        var item = Instantiate(_rankingItemPrefab, transform);
        item.Setup(player);

        var statusPair = new ReadyStatusPair
        {
            Widget = item,
            IsReady = false
        };

        _rankingItems.Add(player.PlayerModel, statusPair);
    }

    private void OnReadyInputPressed(PlayerController controller, bool isReady)
    {
        var statusStruct = _rankingItems[controller.Owner];
        statusStruct.Widget.SetPlayerReady(isReady);
        statusStruct.IsReady = isReady;
        _rankingItems[controller.Owner] = statusStruct;

        bool allReady = true;
        foreach(var pair in _rankingItems.Values)
        {
            if(!pair.IsReady)
            {
                allReady = false;
                break;
            }
        }

        if(allReady)
        {
            GameFacade.AllPlayersReadyForNextGame.Invoke();
        }
    }
}
