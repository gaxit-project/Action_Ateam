using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private ScoreManager scoreManager;

    private int buildIndex;
    public bool IsStart = false;
    public bool isPaused = false;

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

    public int[,] HumanScore;
    public int[,] BotScore;

    [SerializeField] private Vector3 StartPoint = new Vector3(-50f, 0f, 0f);
    [SerializeField] private TextMeshProUGUI testText;

    //private int[] NowFramePoint_0 = new int[11];
    //private int[] NowFramePoint_1 = new int[11];
    //private int[] NowFramePoint_2 = new int[11];
    //private int[] NowFramePoint_3 = new int[11];
    //int pp0,pp1,pp2,pp3;//a-dは仮置き,スコアを表す

    private void Start()
    {
        //testText.text = StartPoint.x.ToString();
    }

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

    /// <summary>
    /// Mainに入ったときにプレイヤーを作成
    /// </summary>
    public void SetUpPlayers()
    {
        if (players == null)
            players = new List<PlayerBase>();

        if (playerScores == null)
            playerScores = new List<PlayerScoreData>();

        if (!IsStart)
        {
            Debug.LogWarning("クリアされたよ");
            players.Clear();
            playerScores.Clear();
        }

        //合計人数
        int totalPlayers = NumHumanPlayers + NumBots;

        //スポーン地点をリストに保存
        List<Vector3> spawnPositions = new List<Vector3>();
        for (int i = 0; i < totalPlayers; i++)
        {
            //現在はx座標を75fずつ左にずらしている状態
            spawnPositions.Add(StartPoint + new Vector3(0f, 0f, i * 2f)); //ここをいじって変えてください
        }

        spawnPositions = spawnPositions.OrderBy(x => Random.value).ToList(); //スポーン場所をランダムに

        int spawnIndex = 0; //人数
        //Player
        for (int i = 0; i < NumHumanPlayers; i++)
        {
            var playerobj = Instantiate(_playerPrefab, spawnPositions[spawnIndex++], Quaternion.identity);
            var player = playerobj.GetComponent<PlayerBase>();
            player.Init($"Player{i + 1}", false);
            players.Add(player);
            if (IsStart == false)
            {
                playerScores.Add(new PlayerScoreData($"Player{i + 1}", false));
            }
            var cam = FindFirstObjectByType<CameraController>();
            if (cam == null) Debug.LogError("nullだよ");
            cam.SetTargetPlayer(player); // Instantiate後に必ず設定！
            var starter = FindFirstObjectByType<GameStarter>();
            if (starter == null) Debug.LogError("starternull");
            starter.SetPlayer(player);

        }

        //Bot
        for (int i = 0; i < NumBots; i++)
        {
            var botobj = Instantiate(_botPrefab, spawnPositions[spawnIndex++], Quaternion.identity);
            var bot = botobj.GetComponent<PlayerBase>();
            if (IsStart == false)
            {
                bot.Init($"Bot{i + 1}", true);
                players.Add(bot);
                playerScores.Add(new PlayerScoreData($"Bot{i + 1}", true));
            }

        }
    }
}

