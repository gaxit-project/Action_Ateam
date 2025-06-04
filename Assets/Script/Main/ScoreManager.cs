using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] int Num_NowFrame = 1;
    [SerializeField] private float displayScoreTime = 3;

    public PinManager pinManager;
    public ResetArea resetArea;

    public GameObject score_object = null;
    public int totalScore;

    [Header("PlayerとbotPrefabと人数")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _botPrefab;
    public int NumHumanPlayers;
    public int NumBots;

    private GameManager gameManager;

    private void Start()
    {
        ResetStart();
        SetUpPlayers();
    }
    private void Update()
    {
        if(gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }
        
        if (pinManager == null || resetArea == null || score_object == null)
        {
            ResetStart();
        }
    }
    public void ResetStart()
    {
        Debug.Log("RESET");
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 144;
        pinManager = FindFirstObjectByType<PinManager>();
        resetArea = FindFirstObjectByType<ResetArea>();
        score_object = GameObject.Find("Canvas/Text (Legacy)");
    }

    /// <summary>
    /// Mainに入ったときにプレイヤーを作成
    /// </summary>
    public void SetUpPlayers()
    {
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
            if (gameManager == null)
            {
                Debug.LogError("GameManager.Instance が null です");
                return;
            }
        }

        if (gameManager.players == null)
            gameManager.players = new List<PlayerBase>();

        if (gameManager.playerScores == null)
            gameManager.playerScores = new List<PlayerScoreData>();

        gameManager.players.Clear();
        gameManager.playerScores.Clear();


        //人間
        for (int i = 0; i < NumHumanPlayers; i++)
        {
            var playerobj = Instantiate(_playerPrefab);
            var player = playerobj.GetComponent<PlayerBase>();
            player.Init($"Player{i + 1}", false);
            gameManager.players.Add(player);
            gameManager.playerScores.Add(new PlayerScoreData($"Player{i + 1}", false));
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
            var botobj = Instantiate(_botPrefab);
            var bot = botobj.GetComponent<PlayerBase>();
            bot.Init($"Bot{i + 1}", true);
            gameManager.players.Add(bot);
            gameManager.playerScores.Add(new PlayerScoreData($"Bot{i + 1}", true));
        }
    }

    /// <summary>
    /// スコアをリストに保存
    /// </summary>
    public void FrameSaveSystem()
    {
        if(gameManager.players == null)
        {
            Debug.Log("player null"); return;
        }
        
        foreach (var player in gameManager.players)
        {
            int pins = pinManager.GetKnockedDownPinCount();

            var scoreData = gameManager.playerScores.Find(p => p.PlayerID == player.GetPlayerID());
            if (scoreData != null)
            {
                scoreData.Addscore(Num_NowFrame, pins);
                Debug.Log($"{scoreData.PlayerID}: {pins} 点（合計 {scoreData.GetTotalScore()}）");
            }
            else
            {
                Debug.Log("scoreDataがnull");
            }
        }

        Num_NowFrame++;
    }

    public void ScoreText()
    {
        string strText;
        Text score_text = score_object.GetComponent<Text>();
        strText = pinManager.GetKnockedDownPinCount().ToString("0");
        score_text.text = strText;
    }

    public IEnumerator DelayAndResetCoroutine()
    {
        yield return new WaitForSeconds(displayScoreTime);
        resetArea.ResetGame();
    }

    public PlayerScoreData GetPlayerScoreData(string playerID)
    {
        return gameManager.playerScores.Find(p => p.PlayerID == playerID);
    }
}
