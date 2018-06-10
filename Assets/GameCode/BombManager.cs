using UnityEngine;

public class BombManager : MonoBehaviour
{
    [SerializeField]
    private Bomb _bombPrefab;

    public void Start()
    {
        GameFacade.BombInputPressed.Connect(OnBombInputPressed);
        GameFacade.BombExploded.Connect(OnBombExploded);
    }

    public void OnDestroy()
    {
        GameFacade.BombInputPressed.Disconnect(OnBombInputPressed);
        GameFacade.BombExploded.Disconnect(OnBombExploded);
    }

    private void OnBombInputPressed(Vector3 position)
    {
        Instantiate(_bombPrefab, position, Quaternion.identity, transform);
    }

    private void OnBombExploded(Bomb bomb)
    {
        Destroy(bomb.gameObject);
    }
}
