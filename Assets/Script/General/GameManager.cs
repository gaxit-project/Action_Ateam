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
    public bool isCounting { private get; set; } = false;

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
    [Header("ステージ番号")]
    public int StageNum;

    public int[,] PlayerScore;

    [SerializeField] private Vector3 StartPoint = new Vector3(-50f, 0f, 0f);
    [SerializeField] private TextMeshProUGUI testText;

    private PointManager pointManager;
    private PinManager pinManager;
    private CameraController cameraController;
    private ResetArea resetArea;

    public ColorAssigner colorAssigner;

    //private int[] NowFramePoint_0 = new int[11];
    //private int[] NowFramePoint_1 = new int[11];
    //private int[] NowFramePoint_2 = new int[11];
    //private int[] NowFramePoint_3 = new int[11];
    //int pp0,pp1,pp2,pp3;//a-dは仮置き,スコアを表す

    private float timer = 10f;
    [SerializeField] private TextMeshProUGUI timerUI;

    private void Start()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow, Screen.currentResolution.refreshRateRatio);//解像度を1980*1080にする
        pointManager = FindFirstObjectByType<PointManager>();
        pinManager = FindFirstObjectByType<PinManager>();
        colorAssigner = FindFirstObjectByType<ColorAssigner>();
        cameraController = GameObject.FindFirstObjectByType<CameraController>();
        resetArea = GameObject.FindFirstObjectByType<ResetArea>();
    }

    private void Update()
    {
        buildIndex = SceneManager.GetActiveScene().buildIndex;
        //Debug.Log(buildIndex);
        if (buildIndex == 1 || buildIndex == 4)
        {
            scoreManager = FindFirstObjectByType<ScoreManager>();
            pointManager = FindFirstObjectByType<PointManager>();
        }

        if(buildIndex == 1 && timerUI == null) timerUI = GameObject.Find("CountDown").GetComponent<TextMeshProUGUI>();

        if (isCounting && timer > -1f)
        {
            timer -= Time.deltaTime;
            if (timer <= -1f)
            {
                timerUI.enabled = false;
                resetArea.TimeOver();
                isCounting = false;
            }
            else
            {
                // 小数点以下を切り上げて整数表示
                int displayTime = Mathf.CeilToInt(timer);
                timerUI.text = displayTime.ToString();
            }
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
        pinManager = FindFirstObjectByType<PinManager>();

        //ステージをランダムに選択
        StageNum = Random.Range(0, 3);
        switch (StageNum)
        {
            case 0:
                StartPoint = new Vector3(-50, -1.25f, 5);
                pinManager.InsertPin(0);
                if (cameraController == null) cameraController = GameObject.FindFirstObjectByType<CameraController>();
                cameraController.throwingCameraPosition = StartPoint - new Vector3(0f, 0f, 5f);
                break;
            case 1:
                StartPoint = new Vector3(33425, -2, 868f);
                pinManager.InsertPin(1);
                if (cameraController == null) cameraController = GameObject.FindFirstObjectByType<CameraController>();
                cameraController.throwingCameraPosition = StartPoint - new Vector3(0f, 0f, 5f);
                break;
            case 2:
                StartPoint = new Vector3(-50, 4, 4982);
                pinManager.InsertPin(2);
                if (cameraController == null) cameraController = GameObject.FindFirstObjectByType<CameraController>();
                cameraController.throwingCameraPosition = StartPoint - new Vector3(0f, 0f, 5f);
                break;

        }

        //スポーン地点をリストに保存
        List<Vector3> spawnPositions = new List<Vector3>();
        for (int i = 0; i < totalPlayers; i++)
        {
            //現在はx座標を75fずつ左にずらしている状態
            spawnPositions.Add(StartPoint + new Vector3(0f, 0f, -i * 3.5f)); //ここをいじって変えてください
        }

        spawnPositions = spawnPositions.OrderBy(x => Random.value).ToList(); //スポーン場所をランダムに

        int spawnIndex = 0; //人数

        //Player
        for (int i = 0; i < NumHumanPlayers; i++)
        {
            var playerobj = Instantiate(_playerPrefab, spawnPositions[spawnIndex++], Quaternion.identity);
            var player = playerobj.GetComponent<Player>();
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
            bot.Init($"Bot{i + 1}", true);
            players.Add(bot);
            if (IsStart == false)
            {
                playerScores.Add(new PlayerScoreData($"Bot{i + 1}", true));
            }

        }

        if (colorAssigner == null)
        {
            colorAssigner = FindFirstObjectByType<ColorAssigner>();
            if (colorAssigner == null)
            {
                Debug.LogError("ColorAssigner がシーン内に存在しません！");
                return;
            }
        }

        colorAssigner.AssignColors();
    }

    public void ResetGame()
    {
        IsStart = false;
        players.Clear();
        playerScores.Clear();
        Num_NowFrame = 1;
        PlayerScore = new int[NumHumanPlayers + NumBots, 11];
    }

    /// <summary>
    /// カウントダウン開始
    /// </summary>
    public void StartCount()
    {
        Debug.Log("カウントスタート!");
        timer = 10f;
        timerUI.enabled = true;
        isCounting = true;
    }

    public void StopTimer()
    {
        timerUI.enabled = false;
        isCounting = false;
    }
}

public static class GlobalColorData
{
    public static readonly List<Color> ColorPalette = new List<Color>
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        Color.cyan,
        Color.magenta,
    };
}