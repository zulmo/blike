using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _meshRenderer;
        
    public void Initialize(Color playerColor)
    {
        _meshRenderer.material = new Material(_meshRenderer.material)
        {
            color = playerColor
        }; 
    }
}
