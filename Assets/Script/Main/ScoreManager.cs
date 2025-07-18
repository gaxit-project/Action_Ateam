using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private float displayScoreTime = 0f;

    public PinManager pinManager;
    public DisplayScore DS;

    public int totalScore;

    private GameManager gameManager;
    private GameStarter gameStarter;
    [SerializeField] PointManager pointManager;

    public bool isFinish = false;

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }
        ResetFrame();
    }
    private void Update()
    {
        if(gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }
        
        if (pinManager == null || DS == null)
        {
            ResetFrame();
        }
    }
    public void ResetFrame()
    {
        //10フレーム目終了(ゲーム終了)
        if (isFinish == true)
        {
            gameManager.IsStart = false;
            SceneChangeManager.Instance.SceneChange("Result");
        }
        //10フレーム以外
        else
        {
            if(gameManager.Num_NowFrame == 10)
            {
                isFinish = true;
            }
            Debug.Log("RESET");
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 144;
            gameStarter = FindFirstObjectByType<GameStarter>();
            pinManager = FindFirstObjectByType<PinManager>();
            DS = FindFirstObjectByType<DisplayScore>();
            gameManager.SetUpPlayers();
            if (pointManager == null) Debug.LogError("ERROR");
            pointManager.PrintAwake();
        }
    }

    /// <summary>
    /// スコアをリストに保存
    /// </summary>
    public void FrameSaveSystem()
    { 
        if(gameManager.players == null)
        {
            Debug.LogError("player null"); return;
        }
        foreach (var player in gameManager.players)
        {
            string id = player.GetPlayerID();
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogError("playerIDが無効です！");
                continue;
            }

            var scoreData = gameManager.playerScores.Find(p => p != null && p.PlayerID == id);

            if (scoreData != null)
            {
                int playerKnockedPins = 0;

                foreach(var pin in pinManager.GetAllPins())
                {
                    playerKnockedPins = pinManager.GetKnockedDownPinCount(id);
                }

                scoreData.Addscore(gameManager.Num_NowFrame, playerKnockedPins);
                //Debug.Log($"{scoreData.PlayerID}: {playerKnockedPins}点(合計{scoreData.GetTotalScore()}) ");
            }
            else
            {
                //Debug.LogWarning($"スコアデータが見つかりません: {id}");
            }
        }

        pointManager.PrintPoint();
        gameManager.Num_NowFrame++;
    }


    public void DelayAndResetCoroutine()
    {
        StartCoroutine(DS.ResetGame());
    }

    /// <summary>
    /// ListからPlayerScoreDataを取得
    /// </summary>
    /// <param name="playerID">PlayerのID</param>
    /// <returns>IDと一致したPlayerScoreData</returns>
    public PlayerScoreData GetPlayerScoreData(string playerID)
    {
        foreach (var p in gameManager.playerScores)
        {
            Debug.Log($"登録されているID: '{p.PlayerID}'");
        }

        var result = gameManager.playerScores.Find(p => p.PlayerID == playerID);
        if (result == null)
        {
            Debug.LogWarning($"'{playerID}' のスコアデータが見つかりません！");
        }
        return result;
    }


}
