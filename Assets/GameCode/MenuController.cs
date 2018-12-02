using UnityEngine;

public class MenuController : MonoBehaviour
{
    private static string[] ActionButtons;
    private static string[] StartButtons;

    static MenuController()
    {
        ActionButtons = new string[BlikeGame.MAX_PLAYERS];
        StartButtons = new string[BlikeGame.MAX_PLAYERS];
        for (int i = 0; i < BlikeGame.MAX_PLAYERS; ++i)
        {
            ActionButtons[i] = string.Format("Player{0}_Action", i+1);
            StartButtons[i] = string.Format("Player{0}_Start", i+1);
        }
    }

    private void FixedUpdate ()
    {
        int playerStartIndex = -1;

        for(int i = 0; i < BlikeGame.MAX_PLAYERS; ++i)
        {
            if (Input.GetButtonUp(ActionButtons[i]))
            {
                MenuFacade.PlayerJoined.Invoke(i);
            }

            if (playerStartIndex < 0 && Input.GetButtonUp(StartButtons[i]))
            {
                playerStartIndex = i;
            }
        }

        if (playerStartIndex >= 0)
        {
            MenuFacade.PlayerStarted.Invoke(playerStartIndex);
        }
    }
}