using UnityEngine;

public class PlayerModel
{
    public int JoystickNumber { get; private set; }
    public Color Color { get; private set; }

    public PlayerModel(int joystick, Color color)
    {
        JoystickNumber = joystick;
        Color = color;
    }
}
