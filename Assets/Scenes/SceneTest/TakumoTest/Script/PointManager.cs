using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class PointManager : MonoBehaviour
{
    [System.Serializable]
    public class TPL
    {
        public List<Text> PL = new List<Text>(); // 1行のText
    }

    [SerializeField] private List<TPL> PT= new List<TPL>();


    public GameManager gameManager;


    IEnumerator Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        if(gameManager == null)
        {
            Debug.LogError("gameManagerがnull");
        }
        if (gameManager.IsStart == false)
        {
            gameManager.HumanScore = new int[gameManager.NumHumanPlayers, 11];
            gameManager.BotScore = new int[gameManager.NumBots, 11];
        }
        yield return new WaitForSeconds(1.5f);
        PrintPoint();
    }



    void PrintPoint()
    {
        for (int i = 0; i < gameManager.NumHumanPlayers; i++)// TPL を4行作成
        {
            var playerData = gameManager.GetPlayerScoreData("Player"+(i+1));
            gameManager.HumanScore[i, gameManager.Num_NowFrame-1] = playerData.FrameScores[gameManager.Num_NowFrame-1];
            Debug.Log(gameManager.HumanScore[i, gameManager.Num_NowFrame - 1]);
            int wholescore = playerData.GetTotalScore();
            for(int j = 1; j < gameManager.Num_NowFrame; j++)
            {
                Debug.Log(i);
                Debug.Log(j);
                Debug.Log(gameManager.HumanScore[i, j]);
                PT[i].PL[j-1].text = gameManager.HumanScore[i, j].ToString();
            }
            if (PT == null)
            {
                Debug.LogError("PTがnullです！");
                return;
            }

            if (i >= PT.Count)
            {
                Debug.LogError($"PTのインデックス{i}が範囲外です！ Count={PT.Count}");
                return;
            }

            if (PT[i].PL == null)
            {
                Debug.LogError($"PT[{i}].PL が null です！");
                return;
            }

            if (PT[i].PL.Count <= 10)
            {
                Debug.LogError($"PT[{i}].PL に十分な Text 要素がありません（Count={PT[i].PL.Count}）");
                return;
            }

            if (PT[i].PL[10] == null)
            {
                Debug.LogError($"PT[{i}].PL[10] が null です（Textコンポーネントが見つからなかった可能性）");
                return;
            }

            // 安全に代入
            PT[i].PL[10].text = wholescore.ToString();

        }

        for (int i = 0;i < gameManager.NumBots;i++)
        {
            var botData = gameManager.GetPlayerScoreData("bot" + (i+1));
            gameManager.BotScore[i, gameManager.Num_NowFrame] = botData.FrameScores[gameManager.Num_NowFrame];
            int wholescore = botData.FrameScores[0];
            for (int j = 1; j < gameManager.Num_NowFrame; j++)
            {
                PT[i].PL[gameManager.Num_NowFrame - j-1].text = gameManager.BotScore[i, j].ToString();
            }


            PT[i].PL[10].text = wholescore.ToString();
        }
       
       
    }






    //Point[1/*1に変数を入れる*/].text = /*1に変数を入れる*/1.ToString();
    //Point[0].text = /*1に変数を入れる*/1.ToString();





}