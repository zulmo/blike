using UnityEngine;

public class MenuController : MonoBehaviour
{
    private static string[] ActionButtons;
    private static string[] StartButtons;

    static MenuController()
    {
        ActionButtons = new string[BlikeGame.MAX_PLAYERS + 1];
        StartButtons = new string[BlikeGame.MAX_PLAYERS + 1];
        for (int i = 0; i <= BlikeGame.MAX_PLAYERS; ++i)
        {
            ActionButtons[i] = string.Format("Player{0}_Action", i);
            StartButtons[i] = string.Format("Player{0}_Start", i);
        }
    }

    private void FixedUpdate ()
    {
        int playerStartIndex = -1;

        for(int i = 0; i <= BlikeGame.MAX_PLAYERS; ++i)
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