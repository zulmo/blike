using UnityEngine;

public static class ScriptableObjectsDatabase
{
    public static PlayerColors PlayerColors { get; private set; }

    public static void Initialize()
    {
        PlayerColors = Resources.Load<PlayerColors>("PlayerColors");
    }
}
