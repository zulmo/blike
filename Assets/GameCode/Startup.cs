using UnityEngine;

public static class Startup
{
    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        ApplicationModels.Initialize();
    }
}
