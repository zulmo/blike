using System;
using UnityEngine;

// https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
public class PlayerController : MonoBehaviour, TileContent
{
    [SerializeField]
    private float _speed = 1;

    [SerializeField]
    private CharacterController _controller;

    public CharacterController CharacterController
    {
        get { return _controller; }
    }

    public PlayerModel Owner { get; private set; }

    public Vector2Int Coords { get; set; }

    private string _horizontalAxis;
    private string _verticalAxis;
    private string _actionButton;


    public void Initialize(PlayerModel owner, Vector2Int coords)
    {
        Owner = owner;
        Coords = coords;

        var joystick = owner.JoystickNumber;
        _horizontalAxis = string.Format("Player{0}_Horizontal", joystick);
        _verticalAxis = string.Format("Player{0}_Vertical", joystick);
        _actionButton = string.Format("Player{0}_Action", joystick);
    }

    void FixedUpdate ()
    {
        // We need to use GetAxisRaw instead of GetAxis because the latter smooths the values, resulting in an unwanted inertia when using the keyboard
        Vector3 moveDirection = new Vector3(Input.GetAxisRaw(_horizontalAxis), 0, Input.GetAxisRaw(_verticalAxis));
        moveDirection = transform.TransformDirection(moveDirection);
        if(!Mathf.Approximately(0f, moveDirection.magnitude))
        {
            moveDirection *= _speed;
            _controller.Move(moveDirection * Time.deltaTime);
            GameFacade.PlayerMoved.Invoke(this);
        }

        if(Input.GetButtonUp(_actionButton))
        {
            GameFacade.BombInputPressed.Invoke(this);
        }
	}

    // TileContent interface
    public TileContentType GetContentType()
    {
        return TileContentType.Player;
    }
}