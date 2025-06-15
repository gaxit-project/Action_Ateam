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

    private void Awake()
    {
        Transform tensu = GameObject.Find("TENSU").transform;

        for (int i = 0; i < tensu.childCount; i++) // name0〜name3
        {
            Transform player = tensu.GetChild(i);
            TPL tpl = new TPL();

            // 各プレイヤーの "waku(x)" を探す
            foreach (Transform child in player)
            {
                if (child.name.StartsWith("waku"))
                {
                    Text scoreText = child.GetComponentInChildren<Text>();
                    if (scoreText != null)
                    {
                        tpl.PL.Add(scoreText);
                    }
                }
            }

            PT.Add(tpl);
        }
    }

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
        yield return new WaitForSeconds(1f);
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
            PT[i].PL[10].text = wholescore.ToString();
        }

        for(int i = 0;i < gameManager.NumBots;i++)
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