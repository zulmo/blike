using System;
using UnityEngine;
using UnityEngine.UI;

public class BlikeMenu : MonoBehaviour
{
    //TODO: Read from Scriptable Object
    private static Color[] PlayerColors = new Color[] { Color.blue, Color.red, Color.green, Color.yellow, Color.black, Color.cyan, Color.magenta, Color.grey };

    [SerializeField]
    private GameObject _playersGrid;

    [SerializeField]
    private Text _joinMessage;

    [SerializeField]
    private Text _startMessage;

    private MenuPlayerSelectItem[] _playersItem;

    private void Awake()
    {
        MenuFacade.PlayerJoined.Connect(OnPlayerJoined);
        MenuFacade.PlayerStarted.Connect(OnPlayerStarted);

        _playersItem = _playersGrid.GetComponentsInChildren<MenuPlayerSelectItem>();

        for(int i = 0, count = _playersItem.Length; i < count; ++i)
        {
            _playersItem[i].Close();
        }

        _startMessage.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        MenuFacade.PlayerJoined.Disconnect(OnPlayerJoined);
        MenuFacade.PlayerStarted.Disconnect(OnPlayerStarted);
    }

    private void OnPlayerJoined(int playerIndex)
    {
        int playerOrder = playerIndex;
        _playersItem[playerOrder].Open(playerIndex, PlayerColors[playerOrder]);
        _startMessage.gameObject.SetActive(true);
        _joinMessage.gameObject.SetActive(playerOrder != BlikeGame.MAX_PLAYERS - 1);
    }

    private void OnPlayerStarted(int playerIndex)
    {
        Debug.Log(string.Format("Game started by player {0}", playerIndex));
    }
}
