using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleResultScreen : MonoBehaviour
{
    [Header("Round End")]
    [SerializeField]
    private RoundEndWidget _roundEndWidget;

    [SerializeField]
    private float _messageDuration;

    [Header("Game Over")]
    [SerializeField]
    private GameEndWidget _gameEndWidget;

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
        _roundEndWidget.Show(winnerIndex);
        _gameEndWidget.gameObject.SetActive(false);
        gameObject.SetActive(true);
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
        _gameEndWidget.Show(winnerIndex);
        _roundEndWidget.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
