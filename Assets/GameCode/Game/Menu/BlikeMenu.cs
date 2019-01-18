using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlikeMenu : MonoBehaviour
{
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

    private void OnPlayerJoined(int joystickNumber)
    {
        var gameModel = ApplicationModels.GetModel<GameModel>();
        if(gameModel.Players.Find(player => player.JoystickNumber == joystickNumber) == null)
        {
            int playerOrder = gameModel.Players.Count;
            var color = ScriptableObjectsDatabase.PlayerColors.Colors[playerOrder];

            _playersItem[playerOrder].Open(playerOrder, color);
            gameModel.AddPlayer(joystickNumber, color);

            _startMessage.gameObject.SetActive(true);
            _joinMessage.gameObject.SetActive(playerOrder != BlikeGame.MAX_PLAYERS - 1);
        }
    }

    private void OnPlayerStarted(int joystickNumber)
    {
        var gameModel = ApplicationModels.GetModel<GameModel>();
        var startPlayer = gameModel.Players.FindIndex(player => player.JoystickNumber == joystickNumber);
        if(startPlayer >= 0)
        {
            Debug.Log(string.Format("Game started by player {0} (joystick {1})", startPlayer + 1, joystickNumber));
        }

        SceneManager.LoadScene("Scenes/TestLevel", LoadSceneMode.Single);
    }
}
