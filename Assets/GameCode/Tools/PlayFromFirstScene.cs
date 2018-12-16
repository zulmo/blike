using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// Taken from https://answers.unity.com/questions/441246/editor-script-to-make-play-always-jump-to-a-start.html
public static class PlayFromFirstScene
{
    private const string PlayFromFirstSceneStr = "Edit/Always Start From Scene 0";

    public static bool Enabled
    {
        get { return EditorPrefs.HasKey(PlayFromFirstSceneStr) && EditorPrefs.GetBool(PlayFromFirstSceneStr); }
        set { EditorPrefs.SetBool(PlayFromFirstSceneStr, value); }
    }

    [MenuItem(PlayFromFirstSceneStr, false, 150)]
    private static void PlayFromFirstSceneCheckMenu()
    {
        Enabled = !Enabled;
        Menu.SetChecked(PlayFromFirstSceneStr, Enabled);

        ShowNotifyOrLog(Enabled ? "Play from scene 0" : "Play from current scene");
    }

    // The menu won't be gray out, we use this validate method for update check state
    [MenuItem(PlayFromFirstSceneStr, true)]
    private static bool PlayFromFirstSceneCheckMenuValidate()
    {
        Menu.SetChecked(PlayFromFirstSceneStr, Enabled);
        return true;
    }

    // This method is called before any Awake. It's the perfect callback for this feature
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void LoadFirstSceneAtGameBegins()
    {
        if (!Enabled)
        {
            return;
        }

        if (EditorBuildSettings.scenes.Length == 0)
        {
            Debug.LogWarning("The scene build list is empty. Can't play from first scene.");
            return;
        }

        SceneManager.LoadScene(0);
    }

    private static void ShowNotifyOrLog(string msg)
    {
        if (Resources.FindObjectsOfTypeAll<SceneView>().Length > 0)
        {
            EditorWindow.GetWindow<SceneView>().ShowNotification(new GUIContent(msg));
        }
        else
        {
            Debug.Log(msg); // When there's no scene view opened, we just print a log
        }
    }
}
