using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private float displayScoreTime = 3;

    public PinManager pinManager;
    public ResetArea resetArea;

    public int totalScore;

    private GameManager gameManager;
    private GameStarter gameStarter;
    [SerializeField] PointManager pointManager;

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }
        ResetGame();
    }
    private void Update()
    {
        if(gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }
        
        if (pinManager == null || resetArea == null)
        {
            ResetGame();
        }
    }
    public void ResetGame()
    {
        //10フレーム目終了(ゲーム終了)
        if (gameManager.Num_NowFrame == 11)
        {
            gameManager.IsStart = false;
            gameManager.Num_NowFrame = 1;
            SceneChangeManager.Instance.SceneChange("Result");
        }
        //10フレーム以外
        else
        {
            Debug.Log("RESET");
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 144;
            gameStarter = FindFirstObjectByType<GameStarter>();
            pinManager = FindFirstObjectByType<PinManager>();
            resetArea = FindFirstObjectByType<ResetArea>();
            gameManager.SetUpPlayers();
            if (pointManager == null) Debug.LogError("ERROR");
            pointManager.PrintPoint();
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
            if (player == null)
            {
                Debug.LogError("playerがnullです！");
                continue;
            }

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
                    if(pin.IsKnockedDownPin(id) == true)
                    {
                        playerKnockedPins++;
                    }
                }

                scoreData.Addscore(gameManager.Num_NowFrame, playerKnockedPins);
                Debug.Log($"{scoreData.PlayerID}: {playerKnockedPins}点(合計{scoreData.GetTotalScore()}) ");
            }
            else
            {
                Debug.LogWarning($"スコアデータが見つかりません: {id}");
            }
        }

        pointManager.PrintPoint();
        gameManager.Num_NowFrame++;
    }


    public IEnumerator DelayAndResetCoroutine()
    {
        yield return new WaitForSeconds(displayScoreTime);
        StartCoroutine(resetArea.ResetGame());
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
