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
        for(int i = 0; i < TestData.NbPlayers; ++i)
        {
            model.AddPlayer(i+1, BlikeMenu.PlayerColors[i]);
        }

        SceneManager.LoadScene(TestData.ScenePath, LoadSceneMode.Single);
    }
}
