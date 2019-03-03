using System.Collections.Generic;

public class BattleModel
{
    public List<PlayerBattleModel> Players { get; private set; }
    
    public BattleModel()
    {
        Players = new List<PlayerBattleModel>();

        var models = ApplicationModels.GetModel<GameModel>().Players;
        for(int i = 0, count = models.Count; i < count; ++i)
        {
            Players.Add(new PlayerBattleModel(models[i]));
        }
    }
}
