using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class BattleTestTool : EditorWindow
{
    private static string ScenePath = "Assets/Scenes/TestLevel.unity";
    private static GUILayoutOption[] EmptyOptions = new GUILayoutOption[0];

    [SerializeField]
    private TestBattleInitializer.Data _data;

    [MenuItem("Tools/Battle Tester")]
    public static void ShowWindow()
    {
        GetWindow(typeof(BattleTestTool));
    }

    private void OnGUI()
    {
        if(_data == null)
        {
            _data = new TestBattleInitializer.Data();
            _data.ScenePath = ScenePath;
        }

        _data.NbPlayers = EditorGUILayout.IntField("Nb players", _data.NbPlayers);
        _data.UseKeyboard = EditorGUILayout.Toggle("Use keyboard", _data.UseKeyboard);
        _data.GameMode = (EGameMode) EditorGUILayout.EnumPopup("Game Mode", _data.GameMode, EmptyOptions);

        if (EditorApplication.isPlaying)
        {
            if (GUILayout.Button("Stop Battle"))
            {
                EditorApplication.isPlaying = false;
            }
        }
        else
        {
            if (GUILayout.Button("Launch Battle"))
            {
                LaunchGame();
            }
        }
    }

    public void LaunchGame()
    {
        var playFirstSceneEnabled = PlayFromFirstScene.Enabled;
        PlayFromFirstScene.Enabled = false;

        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        var initializer = Instantiate(new GameObject()).AddComponent<TestBattleInitializer>();
        initializer.TestData = _data;

        EditorApplication.isPlaying = true;
        PlayFromFirstScene.Enabled = playFirstSceneEnabled;
    }
}
