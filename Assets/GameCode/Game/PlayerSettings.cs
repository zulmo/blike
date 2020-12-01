using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Game Settings/Player Settings", order = 1)]
public class PlayerSettings : ScriptableObject
{
    public PlayerController PlayerPrefab;
    public Bomb BombPrefab;
    public GameObject ExplosionPrefab;
    public List<Color> Colors;
}
