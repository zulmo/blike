public class PlayerBattleFacade
{
    public FacadeEvent<int> ScoreChanged = new FacadeEvent<int>();
    public FacadeEvent Eliminated = new FacadeEvent();
}
