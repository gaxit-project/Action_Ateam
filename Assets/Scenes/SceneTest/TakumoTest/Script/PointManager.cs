using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


    private IEnumerator Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        yield return new WaitForSeconds(1f);
        PointManeger();
    }


    void PointManeger()
    {
       
       
        for (int i = 0; i < gameManager.NumHumanPlayers; i++)// TPL を4行作成
        {
            var playerData = gameManager.GetPlayerScoreData("Player"+ (i+1));
            int score = playerData.FrameScores[gameManager.Num_NowFrame-1];
            int wholescore = playerData.FrameScores[0];
            PT[i].PL[gameManager.Num_NowFrame].text = score.ToString();
            PT[i].PL[10].text = wholescore.ToString();
        }

        for(int i = 0;i < gameManager.NumBots;i++)
        {
            var botData = gameManager.GetPlayerScoreData("bot" + (i+1));
            int score = botData.FrameScores[gameManager.Num_NowFrame-1];
            int wholescore = botData.FrameScores[0];
            PT[i].PL[gameManager.Num_NowFrame].text = score.ToString();
            PT[i].PL[10].text = wholescore.ToString();
        }
       
       
    }






    //Point[1/*1に変数を入れる*/].text = /*1に変数を入れる*/1.ToString();
    //Point[0].text = /*1に変数を入れる*/1.ToString();





}