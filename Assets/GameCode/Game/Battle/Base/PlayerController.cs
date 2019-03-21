using UnityEngine;

// https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
public class PlayerController : MonoBehaviour, TileContent
{
    public enum InputStatus
    {
        Blocked,
        Playing,
        GameEnd_NotReady,
        GameEnd_Ready
    }

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

    public InputStatus Status { get; set; }

    private string _horizontalAxis;
    private string _verticalAxis;
    private string _actionButton;


    public void Initialize(PlayerModel owner)
    {
        Owner = owner;

        var joystick = owner.JoystickNumber;
        _horizontalAxis = string.Format("Player{0}_Horizontal", joystick);
        _verticalAxis = string.Format("Player{0}_Vertical", joystick);
        _actionButton = string.Format("Player{0}_Action", joystick);

        Status = InputStatus.Blocked;
    }

    void FixedUpdate()
    {
        switch(Status)
        {
            case InputStatus.Playing:
            {
                // We need to use GetAxisRaw instead of GetAxis because the latter smooths the values, resulting in an unwanted inertia when using the keyboard
                Vector3 moveDirection = new Vector3(Input.GetAxisRaw(_horizontalAxis), 0, Input.GetAxisRaw(_verticalAxis));
                moveDirection = transform.TransformDirection(moveDirection);
                if (!Mathf.Approximately(0f, moveDirection.magnitude))
                {
                    moveDirection *= _speed;
                    _controller.Move(moveDirection * Time.deltaTime);
                    GameFacade.PlayerMoved.Invoke(this);
                }

                if (Input.GetButtonUp(_actionButton))
                {
                    GameFacade.BombInputPressed.Invoke(this);
                }
            }
            break;

            // Maybe we want to allow input only if the player is not ready yet? 
            case InputStatus.GameEnd_NotReady:
            case InputStatus.GameEnd_Ready:
            {                
                if (Input.GetButtonUp(_actionButton))
                {
                    bool wasReady = Status == InputStatus.GameEnd_Ready;
                    Status = wasReady ? InputStatus.GameEnd_NotReady : InputStatus.GameEnd_Ready;
                    GameFacade.ReadyInputPressed.Invoke(this, !wasReady);
                }
            }
            break;
        }
	}

    // TileContent interface
    public TileContentType GetContentType()
    {
        return TileContentType.Player;
    }
}