using UnityEngine;

// https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
public class PlayerController : MonoBehaviour, TileContent
{
    [SerializeField]
    private float _speed = 1;

    [SerializeField]
    private CharacterController _controller;

    public Vector2Int Coords { get; set; }

    void FixedUpdate ()
    {
        // We need to use GetAxisRaw instead of GetAxis because the latter smooths the values, resulting in an unwanted inertia when usnig the keyboard
        Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        if(!Mathf.Approximately(0f, moveDirection.magnitude))
        {
            moveDirection *= _speed;
            _controller.Move(moveDirection * Time.deltaTime);
            GameFacade.PlayerMoved.Invoke(this);
        }

        if(Input.GetButtonUp("Action"))
        {
            GameFacade.BombInputPressed.Invoke(transform.position);
        }
	}

    // TileContent interface
    public TileContentType GetContentType()
    {
        return TileContentType.Player;
    }
}