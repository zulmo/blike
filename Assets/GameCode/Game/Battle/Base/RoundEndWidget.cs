using System;
using UnityEngine;
using UnityEngine.UI;

public class RoundEndWidget : MonoBehaviour
{
    private static string WinTextFormat = "PLAYER {0} WINS THE ROUND!";

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

    public void Show(int winnerIndex)
    {
        bool hasWinner = winnerIndex >= 0;

        _winRoot.SetActive(hasWinner);
        _drawRoot.SetActive(!hasWinner);

        if (hasWinner)
        {
            var playerModel = ApplicationModels.GetModel<GameModel>().Players[winnerIndex];
            _winnerColor.color = playerModel.Color;
            _winText.text = string.Format(WinTextFormat, winnerIndex + 1);
        }
    }
}
