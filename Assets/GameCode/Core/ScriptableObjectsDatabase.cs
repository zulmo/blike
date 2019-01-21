using UnityEngine;

public static class ScriptableObjectsDatabase
{
    public static PlayerSettings PlayerSettings { get; private set; }

    public static void Initialize()
    {
        PlayerSettings = Resources.Load<PlayerSettings>("PlayerSettings");
    }
}
