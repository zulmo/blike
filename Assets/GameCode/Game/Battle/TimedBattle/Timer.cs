using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private static string TimerFormat = "{0:00}:{1:00}";

    [SerializeField]
    private Text _text;

    public void UpdateTime(int remainingSeconds)
    {
        var minutes = remainingSeconds / 60;
        var seconds = remainingSeconds % 60;
        _text.text = string.Format(TimerFormat, minutes, seconds);
    }
}
