using UnityEngine;
using UnityEngine.UI;

public class PlayerReadyStatusWidget : MonoBehaviour
{
    [SerializeField]
    private RawImage _imageComponent;

    [SerializeField]
    private Texture _readyTexture;

    [SerializeField]
    private Texture _notReadyTexture;

    public void SetReady(bool ready)
    {
        _imageComponent.texture = ready ? _readyTexture : _notReadyTexture;
    }
}
