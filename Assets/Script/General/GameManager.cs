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
    [SerializeField] private float displayScoreTime = 3;
    [SerializeField] int Num_NowFrame=1;
    

    public PinManager pinManager;
    public ResetArea resetArea;

    public GameObject score_object = null;
    public int totalScore;

    [Header("PlayerのPrefabと人数")]
    [SerializeField] private GameObject _playerPrefab;
    public int NumHumanPlayers;
    public int NumBots;

    //Playerのスコア管理
    private List<PlayerBase> players = new List<PlayerBase>();
    private List<PlayerScoreData> playerScores = new List<PlayerScoreData>();

    //private int[] NowFramePoint_0 = new int[11];
    //private int[] NowFramePoint_1 = new int[11];
    //private int[] NowFramePoint_2 = new int[11];
    //private int[] NowFramePoint_3 = new int[11];
    //int pp0,pp1,pp2,pp3;//a-dは仮置き,スコアを表す


    public void ResetStart()
    {
        Debug.Log("RESET");
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 144;
        pinManager = FindFirstObjectByType<PinManager>();
        resetArea = FindFirstObjectByType<ResetArea>();
        score_object = GameObject.Find("Canvas/Text (Legacy)");
    }

    private void Start()
    {
        SetUpPlayers();
    }
    private void Update()
    {
        if (false/*resetArea.OnTriggerEnter*/)
        {
            FrameSaveSystem();
        }
        if (pinManager == null || resetArea == null || score_object == null)
        {
            ResetStart();
        }
    }

    /// <summary>
    /// Mainに入ったときにプレイヤーを作成
    /// </summary>
    public void SetUpPlayers()
    {
        players.Clear();
        playerScores.Clear();

        //人間
        for (int i = 0; i < NumHumanPlayers; i++)
        {
            var playerobj = Instantiate(_playerPrefab);
            var player = playerobj.GetComponent<PlayerBase>();
            player.Init($"Player{i + 1}", false);
            players.Add(player);
            playerScores.Add(new PlayerScoreData($"Player{i + 1}", false));
            var cam = FindFirstObjectByType<CameraController>();
            //if (cam == null) Debug.LogError("nullだよ");
            cam.SetTargetPlayer(player); // Instantiate後に必ず設定！
            var starter = FindFirstObjectByType<GameStarter>();
            //if (starter == null) Debug.LogError("starternull");
            starter.SetPlayer(player);

        }

        //Bot
        for (int i = 0; i < NumBots; i++)
        {
            var botobj = Instantiate(_playerPrefab);
            var bot = botobj.GetComponent<PlayerBase>().GetComponent<PlayerBase>();
            bot.Init($"Bot{i + 1}", true);
            players.Add(bot);
            playerScores.Add(new PlayerScoreData($"Bot{i + 1}", true));
        }
    }

    /// <summary>
    /// スコアをリストに保存
    /// </summary>
    public void FrameSaveSystem()
    {
        foreach (var player in players)
        {
            int pins = pinManager.GetKnockedDownPinCount();

            var scoreData = playerScores.Find(p => p.PlayerID == player.GetPlayerID());
            if (scoreData != null)
            {
                scoreData.Addscore(Num_NowFrame, pins);
                Debug.Log($"{scoreData.PlayerID}: {pins} 点（合計 {scoreData.GetTotalScore()}）");
            }
        }

        Num_NowFrame++;
        resetArea.ResetGame();
    }


    public void CurrentFrameResult()
    {
        ScoreText();
        StartCoroutine(DelayAndResetCoroutine());
    }

    public void ScoreText()
    {
        string strText;
        Text score_text = score_object.GetComponent<Text>();
        strText = pinManager.GetKnockedDownPinCount().ToString("0");
        score_text.text = strText;
    }

    private IEnumerator DelayAndResetCoroutine()
    {
        yield return new WaitForSeconds(displayScoreTime);
        resetArea.ResetGame();
    }
}
