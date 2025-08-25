using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Player2;

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

    [SerializeField] public Vector3 StartPoint = new Vector3(-50f, 0f, 0f);
    [SerializeField] private TextMeshProUGUI testText;

    private PointManager pointManager;
    private PinManager pinManager;
    private CameraController cameraController;
    private DisplayScore DS;

    public ColorAssigner colorAssigner;

    //private int[] NowFramePoint_0 = new int[11];
    //private int[] NowFramePoint_1 = new int[11];
    //private int[] NowFramePoint_2 = new int[11];
    //private int[] NowFramePoint_3 = new int[11];
    //int pp0,pp1,pp2,pp3;//a-dは仮置き,スコアを表す

    //Timer関連
    private float maxTime = 11f;
    private float remainingTime = 10f;
    private GameObject timerUI;
    private TextMeshProUGUI timer;
    private Image image;
    private bool isGettingTimer = false;

    private void Start()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow, Screen.currentResolution.refreshRateRatio);//解像度を1980*1080にする
        pointManager = FindFirstObjectByType<PointManager>();
        pinManager = FindFirstObjectByType<PinManager>();
        colorAssigner = FindFirstObjectByType<ColorAssigner>();
        cameraController = GameObject.FindFirstObjectByType<CameraController>();
        DS = GameObject.FindFirstObjectByType<DisplayScore>();
    }

    private void Update()
    {
        buildIndex = SceneManager.GetActiveScene().buildIndex;
        //Debug.Log(buildIndex);
        if (DS == null && buildIndex == 1) DS = GameObject.FindFirstObjectByType<DisplayScore>();

        if (buildIndex == 1 || buildIndex == 4)
        {
            scoreManager = FindFirstObjectByType<ScoreManager>();
            pointManager = FindFirstObjectByType<PointManager>();
        }

        if (isCounting && remainingTime > -1f)
        {
            remainingTime -= Time.deltaTime;
            image.fillAmount = (remainingTime + 1) / maxTime > 0f ? (remainingTime + 1) / maxTime : 0f;
            if (remainingTime <= -1f)
            {
                timerUI.SetActive(false);
                DS.TimeOver();
                isCounting = false;
            }
            else
            {
                // 小数点以下を切り上げて整数表示
                int displayTime = Mathf.CeilToInt(remainingTime);
                timer.text = displayTime.ToString();
            }
        }
    }


    public void CurrentFrameResult()
    {
        pointManager.PrintName();
        scoreManager.DelayAndResetCoroutine();
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
        if(Num_NowFrame <= 3) StageNum = Num_NowFrame - 1;
        else StageNum = Random.Range(0, 3);
        switch (StageNum)
        {
            case 0:
                StartPoint = new Vector3(-50, -1.25f, 0);
                pinManager.InsertPin(0);
                if (cameraController == null) cameraController = GameObject.FindFirstObjectByType<CameraController>();
                cameraController.throwingCameraPosition = StartPoint - new Vector3(0f, 0f, 5f);
                AudioManager.Instance.PlayBGM(3);
                break;
            case 1:
                StartPoint = new Vector3(33425, -2, 863);
                pinManager.InsertPin(1);
                if (cameraController == null) cameraController = GameObject.FindFirstObjectByType<CameraController>();
                cameraController.throwingCameraPosition = StartPoint - new Vector3(0f, 0f, 5f);
                AudioManager.Instance.PlayBGM(4);
                break;
            case 2:
                StartPoint = new Vector3(-50, 4, 4977);
                pinManager.InsertPin(2);
                if (cameraController == null) cameraController = GameObject.FindFirstObjectByType<CameraController>();
                cameraController.throwingCameraPosition = StartPoint - new Vector3(0f, 0f, 5f);
                AudioManager.Instance.PlayBGM(5);
                break;

        }

        //スポーン地点をリストに保存
        List<Vector3> spawnPositions = new List<Vector3>();
        for (int i = 0; i < totalPlayers; i++)
        {
            spawnPositions.Add(StartPoint + new Vector3(10f, 0f, (2 - i) * 5f - 2.5f));
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
            cam.SetTargetPlayer(player, StartPoint); // Instantiate後に必ず設定！
            var starter = FindFirstObjectByType<GameStarter>();
            if (starter == null) Debug.LogError("starternull");
            starter.SetPlayer(player);
            player.LoadScore();
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
            bot.LoadScore();
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
        CrownScript crownScript = FindFirstObjectByType<CrownScript>();
        crownScript.CrownMove();

        timerUI = GameObject.Find("Timer");
        timer = GameObject.Find("CountDown").GetComponent<TextMeshProUGUI>();
        image = GameObject.Find("Ring").GetComponent<Image>();
        if (timerUI != null && timer != null && image != null)
        {
            isGettingTimer = true;
            timerUI.SetActive(false);
        }
        else Debug.LogError("1つあるいは複数のタイマー関連のオブジェクトが取得できませんでした");

        SelectArea selectArea = FindFirstObjectByType<SelectArea>();
        selectArea.AddList();
    }

    public void ResultSetting()
    {
        //Player
        for (int i = 0; i < NumHumanPlayers; i++)
        {
            var playerobj = Instantiate(_playerPrefab, new Vector3(), Quaternion.Euler(0f, 270f, 0f));
            var player = playerobj.GetComponent<Player>();
            player.Init($"Player{i + 1}", false);
            players.Add(player);
            var cam = FindFirstObjectByType<CameraController>();
            if (cam == null) Debug.LogError("nullだよ");
            cam.SetTargetPlayer(player, StartPoint); // Instantiate後に必ず設定！
        }
        //Bot
        for (int i = 0; i < NumBots; i++)
        {
            var botobj = Instantiate(_botPrefab, new Vector3(), Quaternion.Euler(0f, 270f, 0f));
            var bot = botobj.GetComponent<PlayerBase>();
            bot.Init($"Bot{i + 1}", true);
            players.Add(bot);

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
        RankSort();
    }

    public void CameraControl()
    {
        switch (StageNum)
        {
            case 0:
                if (cameraController == null) cameraController = GameObject.FindFirstObjectByType<CameraController>();
                cameraController.throwingCameraPosition = StartPoint - new Vector3(0f, 0f, 5f);
                break;
            case 1:
                if (cameraController == null) cameraController = GameObject.FindFirstObjectByType<CameraController>();
                cameraController.throwingCameraPosition = StartPoint - new Vector3(0f, 0f, 5f);
                break;
            case 2:
                if (cameraController == null) cameraController = GameObject.FindFirstObjectByType<CameraController>();
                cameraController.throwingCameraPosition = StartPoint - new Vector3(0f, 0f, 5f);
                break;
        }

        foreach(var p in players)
        {
            if(p.IsBot == false)
            {
                var player = p.GetComponent<Player>();
                var cam = FindFirstObjectByType<CameraController>();
                if (cam == null) Debug.LogError("nullだよ");
                cam.SetTargetPlayer(player, StartPoint); // Instantiate後に必ず設定！
            }
        }
 


    }

    public void ResetGame()
    {
        IsStart = false;
        players.Clear();
        playerScores.Clear();
        Num_NowFrame = 1;
        PlayerScore = new int[NumHumanPlayers + NumBots, 11];
        isCounting = false;
    }

    /// <summary>
    /// カウントダウン開始
    /// </summary>
    public void StartCount()
    {
        Debug.Log("カウントスタート!");
        timerUI.SetActive(true);
        remainingTime = 10f;
        isCounting = true;
    }

    public void StopTimer()
    {
        timerUI.SetActive(false);
        isCounting = false;
    }

    public int GetRankByID(string playerID)
    {
        var player = players.Find(p => p.PlayerID == playerID);
        if (player != null)
        {
            return player.Rank;
        }
        else
        {
            Debug.Log($"{playerID}のプレイヤーが見つかりませんでした!");
            return -1;
        }
    }

    public void RankSort()
    {
        // プレイヤーをスコアの降順でソート
        var sorted = players.OrderByDescending(p =>
        {
            var scoreData = GetPlayerScoreData(p.PlayerID);
            // scoreDataが見つからない場合は0点として扱う（エラーログは別途出すべき）
            return scoreData != null ? scoreData.GetTotalScore() : 0;
        }).ToList();

        int currentRank = 1;
        int previousScore = -1; // 前のプレイヤーのスコアを保持

        for (int i = 0; i < sorted.Count; i++)
        {
            var currentPlayer = sorted[i];
            var currentPlayerScoreData = GetPlayerScoreData(currentPlayer.PlayerID);
            int currentPlayerTotalScore = currentPlayerScoreData != null ? currentPlayerScoreData.GetTotalScore() : 0;

            // 最初のプレイヤー、または前のプレイヤーとスコアが異なる場合
            if (i == 0 || currentPlayerTotalScore < previousScore)
            {
                currentRank = i + 1; // 順位を更新
            }
            // スコアが同じ場合は同じ順位を適用
            // else if (currentPlayerTotalScore == previousScore) {
            //   // 何もしない。currentRankは前のプレイヤーと同じ。
            // }

            currentPlayer.SetRank(currentRank); // プレイヤーにランクを設定
            Debug.Log($"Player: {currentPlayer.PlayerID}, Score: {currentPlayerTotalScore}, Rank: {currentPlayer.Rank}"); // デバッグログ

            previousScore = currentPlayerTotalScore; // 現在のスコアを次のループのために保存
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
}