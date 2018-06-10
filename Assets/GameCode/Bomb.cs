using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    private float Timer;

    [SerializeField]
    private int Range;

    private float Elapsed = 0;

	void Update ()
    {
        Elapsed += Time.deltaTime;
        if(Elapsed > Timer)
        {
            GameFacade.BombExploded.Invoke(this);
        }
	}
}
