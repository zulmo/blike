using UnityEngine;

[CreateAssetMenu(fileName = "GameViews", menuName = "Game Settings/Views", order = 2)]
public class GameViews : ScriptableObject
{
    [SerializeField]
    private ViewsDictionary _views = new ViewsDictionary();
    
    public GameView this[EGameMode mode]
    {
        get
        {
            return _views[mode];
        }
    }
}