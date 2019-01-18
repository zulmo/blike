using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerColors", menuName = "Game Settings/Player Colors", order = 1)]
public class PlayerColors : ScriptableObject
{
    public Material Material;
    public List<Color> Colors;
}
