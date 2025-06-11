using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private ScoreManager scoreManager;

    private int buildIndex;
    public bool IsStart = false;

    //現在のフレーム
    public int Num_NowFrame = 1;

    //Playerのスコア管理
    public List<PlayerBase> players = new List<PlayerBase>();
    public List<PlayerScoreData> playerScores = new List<PlayerScoreData>();

    [Header("PlayerとbotPrefabと人数")]
    public GameObject _playerPrefab;
    public GameObject _botPrefab;
    public int NumHumanPlayers;
    public int NumBots;

    //private int[] NowFramePoint_0 = new int[11];
    //private int[] NowFramePoint_1 = new int[11];
    //private int[] NowFramePoint_2 = new int[11];
    //private int[] NowFramePoint_3 = new int[11];
    //int pp0,pp1,pp2,pp3;//a-dは仮置き,スコアを表す

    private void Update()
    {
        buildIndex = SceneManager.GetActiveScene().buildIndex;
        //Debug.Log(buildIndex);
        if (buildIndex == 1 || buildIndex == 4)
        {
            scoreManager = FindFirstObjectByType<ScoreManager>();
        }
    }


    public void CurrentFrameResult()
    {
        scoreManager.ScoreText();
        StartCoroutine(scoreManager.DelayAndResetCoroutine());
    }

    public PlayerScoreData GetPlayerScoreData(string playerID)
    {
        foreach (var p in playerScores)
        {
            Debug.Log($"登録されているID: '{p.PlayerID}'");
        }

        var result = playerScores.Find(p => p.PlayerID == playerID);
        if (result == null)
        {
            Debug.LogWarning($"'{playerID}' のスコアデータが見つかりません！");
        }
        return result;
    }
}

