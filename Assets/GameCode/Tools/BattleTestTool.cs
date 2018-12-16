using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class BattleTestTool : EditorWindow
{
    private static string ScenePath = "Assets/Scenes/TestLevel.unity";

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
        if(EditorApplication.isPlaying)
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
