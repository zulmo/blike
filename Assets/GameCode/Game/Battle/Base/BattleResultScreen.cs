using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleResultScreen : MonoBehaviour
{
    private static string WinRoundTextFormat = "PLAYER {0} WINS THE ROUND!";
    private static string WinGameTextFormat = "PLAYER {0} WINS THE GAME!";

    [Header("Round End")]
    [SerializeField]
    private float _messageDuration;

    [Header("Win Layout")]
    [SerializeField]
    private GameObject _winRoot;

    [SerializeField]
    private Image _winnerColor;

    [SerializeField]
    private Text _winText;

    [Header("Draw Layout")]
    [SerializeField]
    private GameObject _drawRoot;

    public void Initialize()
    {
        GameFacade.RoundOver.Connect(OnRoundOver);
        GameFacade.GameOver.Connect(OnGameOver);
    }

    public void Deinitialize()
    {
        GameFacade.RoundOver.Disconnect(OnRoundOver);
        GameFacade.GameOver.Disconnect(OnGameOver);
    }

    public void OnRoundOver(int winnerIndex, Action callback)
    {
        SetupLayout(winnerIndex, WinRoundTextFormat);
        StartCoroutine(WaitAndHide(callback));
    }

    private IEnumerator WaitAndHide(Action callback)
    {
        yield return new WaitForSeconds(_messageDuration);
        gameObject.SetActive(false);
        callback();
    }

    public void OnGameOver(int winnerIndex)
    {
        SetupLayout(winnerIndex, WinGameTextFormat);
    }

    private void SetupLayout(int winnerIndex, string winTextFormat)
    {
        bool hasWinner = winnerIndex >= 0;

        _winRoot.SetActive(hasWinner);
        _drawRoot.SetActive(!hasWinner);

        if(hasWinner)
        {
            var playerModel = ApplicationModels.GetModel<GameModel>().Players[winnerIndex];
            _winnerColor.color = playerModel.Color;
            _winText.text = string.Format(winTextFormat, winnerIndex + 1);
        }

        gameObject.SetActive(true);
    }
}
