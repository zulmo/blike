using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestBattleInitializer : MonoBehaviour
{
    [Serializable]
    public class Data
    {
        [SerializeField]
        private int _nbPlayers;
        public int NbPlayers
        {
            get { return _nbPlayers; }
            set { _nbPlayers = value; }
        }

        [SerializeField]
        private bool _useKeyboard = true;
        public bool UseKeyboard
        {
            get { return _useKeyboard; }
            set { _useKeyboard = value; }
        }

        [SerializeField]
        private string _scenePath;
        public string ScenePath
        {
            get { return _scenePath; }
            set { _scenePath = value; }
        }
    }

    [SerializeField]
    private Data _testData;
    public Data TestData
    {
        get { return _testData; }
        set { _testData = value; }
    }

    private void Awake()
    {
        var model = ApplicationModels.GetModel<GameModel>();
        var joystickOffset = TestData.UseKeyboard ? 0 : 1;
        for(int i = 0; i < TestData.NbPlayers; ++i)
        {
            model.AddPlayer(i + joystickOffset, ScriptableObjectsDatabase.PlayerColors.Colors[i]);
        }

        SceneManager.LoadScene(TestData.ScenePath, LoadSceneMode.Single);
    }
}
