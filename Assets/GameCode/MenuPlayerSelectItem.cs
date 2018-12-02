using UnityEngine;
using UnityEngine.UI;

public class MenuPlayerSelectItem : MonoBehaviour
{
    [SerializeField]
    private Text _playerText;

    [SerializeField]
    private Image _playerColor;

    private void Awake()
    {
        _playerColor.material = new Material(_playerColor.material);
    }

    public void Open(int playerIndex, Color color)
    {
        _playerText.text = string.Format("Player {0}", playerIndex + 1);
        _playerColor.gameObject.SetActive(true);
        _playerColor.material.color = color;
    }

    public void Close()
    {
        _playerText.text = "Join! (A)";
        _playerColor.gameObject.SetActive(false);
    }
}
