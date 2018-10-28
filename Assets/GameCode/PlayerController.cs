using UnityEngine;

// https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
public class PlayerController : MonoBehaviour, TileContent
{
    [SerializeField]
    private float _speed = 1;

    [SerializeField]
    private CharacterController _controller;

    private int _playerIndex;
    public int PlayerIndex
    {
        get
        {
            return _playerIndex;
        }

        set
        {
            _playerIndex = value;
            SetupAxes();
        }
    }

    public Vector2Int Coords { get; set; }

    private string _horizontalAxis;
    private string _verticalAxis;
    private string _actionButton;
    
    private void SetupAxes()
    {
        _horizontalAxis = string.Format("Player{0}_Horizontal", PlayerIndex);
        _verticalAxis = string.Format("Player{0}_Vertical", PlayerIndex);
        _actionButton = string.Format("Player{0}_Action", PlayerIndex);
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
            GameFacade.BombInputPressed.Invoke(transform.position);
        }
	}

    // TileContent interface
    public TileContentType GetContentType()
    {
        return TileContentType.Player;
    }
}