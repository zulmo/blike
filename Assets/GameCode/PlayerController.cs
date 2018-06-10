using UnityEngine;

// https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1;

    [SerializeField]
    private CharacterController _controller;

    void FixedUpdate ()
    {
        // We need to use GetAxisRaw instead of GetAxis because the latter smooths the values, resulting in an unwanted inertia when usnig the keyboard
        Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= _speed;
        _controller.Move(moveDirection * Time.deltaTime);

        if(Input.GetButtonUp("Action"))
        {
            GameFacade.BombInputPressed.Invoke(transform.position);
        }
	}
}