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

    public GameObject score_object = null;
    public int totalScore;

    private GameManager gameManager;
    private GameStarter gameStarter;

    private void Start()
    {
            ResetStart();  
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
        gameStarter = FindFirstObjectByType<GameStarter>();
        pinManager = FindFirstObjectByType<PinManager>();
        resetArea = FindFirstObjectByType<ResetArea>();
        score_object = GameObject.Find("Canvas/Text (Legacy)");
        gameStarter.SetUpPlayers();
    }



    /// <summary>
    /// スコアをリストに保存
    /// </summary>
    public void FrameSaveSystem()
    {
        gameManager.players.RemoveAll(p => p == null);

        if(gameManager.players == null)
        {
            Debug.Log("player null"); return;
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
                int pins = pinManager.GetKnockedDownPinCount();
                scoreData.Addscore(gameManager.Num_NowFrame, pins);
                Debug.Log($"{scoreData.PlayerID}: {pins} 点（合計 {scoreData.GetTotalScore()}）");
            }
            else
            {
                Debug.LogWarning($"スコアデータが見つかりません: {id}");
            }
        }


        gameManager.Num_NowFrame++;
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
