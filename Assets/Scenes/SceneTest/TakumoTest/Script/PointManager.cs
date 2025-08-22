using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
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
    [SerializeField] private List<Text> Name = new List<Text>();
    private int TotalPlayer;
    private string[] PlayerName;
    private Color[] PlayerColors;


    public GameManager gameManager;


    void Awake()
    {
        gameManager = GameManager.Instance;
        if(gameManager == null)
        {
            Debug.LogError("gameManagerがnull");
        }
        if (gameManager.IsStart == false && gameManager.Num_NowFrame == 1)
        {
            TotalPlayer = gameManager.NumHumanPlayers + gameManager.NumBots;
            gameManager.PlayerScore = new int[TotalPlayer, 11];
        }
        TotalPlayer = gameManager.NumHumanPlayers + gameManager.NumBots;
    }

    public void PrintName()
    {
        PlayerName = new string[4];
        PlayerColors = new Color[4];
        int num = 0;
        foreach (var player in gameManager.players)
        {
            if(player.IsBot == false)
            {
                PlayerName[num] = "You";
                PlayerColors[num] = player.GetPlayerColor();
                num++;
            }
            else
            {
                string colorName = player.GetColorName();
                PlayerName[num] = colorName;
                PlayerColors[num] = player.GetPlayerColor();
                num++;
            }

        }



        for (int i = 0; i < TotalPlayer; i++)
        {
            if (PlayerName[i] == "YOU")
            {
                Name[i].text = PlayerName[i];
                Name[i].color = PlayerColors[i];
            }
            else
            {
                Name[i].text = PlayerName[i];
                Name[i].color = PlayerColors[i];
            }

        }
    }

    public void PrintAwake()
    {


        for (int i = 0; i < gameManager.NumHumanPlayers; i++)// TPL を4行作成
        {
            var playerData = gameManager.GetPlayerScoreData("Player" + (i + 1));
            if (gameManager == null) Debug.LogError("GameManagerNULL");
            gameManager.PlayerScore[i, gameManager.Num_NowFrame] = playerData.GetScore(gameManager.Num_NowFrame);
            //Debug.log(gameManager.PlayerScore[i, gameManager.Num_NowFrame]);
            int wholescore = playerData.GetTotalScore();
            gameManager.PlayerScore[i, 0] = wholescore;
            for (int j = 1; j < gameManager.Num_NowFrame; j++)
            {
                PT[i].PL[j - 1].text = gameManager.PlayerScore[i, j].ToString();
            }

            // 安全に代入
            PT[i].PL[10].text = wholescore.ToString();

        }

        int idNum = 0;

        for (int i = gameManager.NumHumanPlayers; i < TotalPlayer; i++)
        {
            var botData = gameManager.GetPlayerScoreData("Bot" + (idNum + 1));
            gameManager.PlayerScore[i, gameManager.Num_NowFrame] = botData.GetScore(gameManager.Num_NowFrame);
            int wholescore = botData.GetTotalScore();
            gameManager.PlayerScore[i, 0] = wholescore;
            for (int j = 1; j < gameManager.Num_NowFrame; j++)
            {
                PT[i].PL[j - 1].text = gameManager.PlayerScore[i, j].ToString();
            }
            // 安全に代入
            PT[i].PL[10].text = wholescore.ToString();
            idNum++;
        }

    }
    public void PrintPoint()
    {   
        

        for (int i = 0; i < gameManager.NumHumanPlayers; i++)// TPL を4行作成
        {
            var playerData = gameManager.GetPlayerScoreData("Player"+(i+1));
            if (gameManager == null) Debug.LogError("GameManagerNULL");
            gameManager.PlayerScore[i, gameManager.Num_NowFrame] = playerData.GetScore(gameManager.Num_NowFrame);
            //Debug.log(gameManager.PlayerScore[i, gameManager.Num_NowFrame]);
            int wholescore = playerData.GetTotalScore();
            gameManager.PlayerScore[i, 0] = wholescore;
            for (int j = 1; j < gameManager.Num_NowFrame+1; j++)
            {
                PT[i].PL[j-1].text = gameManager.PlayerScore[i, j].ToString();
            }

            // 安全に代入
            PT[i].PL[10].text = wholescore.ToString();

        }
        
        int idNum = 0;

        for (int i = gameManager.NumHumanPlayers ;i < TotalPlayer; i++)
        {
            var botData = gameManager.GetPlayerScoreData("Bot" + (idNum+1));
            gameManager.PlayerScore[i, gameManager.Num_NowFrame] = botData.GetScore(gameManager.Num_NowFrame);
            int wholescore = botData.GetTotalScore();
            gameManager.PlayerScore[i, 0] = wholescore;
            for (int j = 1; j < gameManager.Num_NowFrame + 1; j++)
            {
                PT[i].PL[j - 1].text = gameManager.PlayerScore[i, j].ToString();
            }
            // 安全に代入
            PT[i].PL[10].text = wholescore.ToString();
            idNum++;
        }
       
    }

    /// <summary>
    /// リザルトシーンで点数をプリント
    /// </summary>
    public void ResultPrint()
    {
        PlayerName = new string[4];
        int num = 0;
        foreach (var player in gameManager.players)
        {
            string id = player.GetPlayerID();
            PlayerName[num] = id;
            num++;
        }

        for (int i = 0; i < TotalPlayer; i++)
        {
            if (PlayerName == null) Debug.LogError("PlayerNameError");
            if (Name[i] == null) Debug.LogError("NameError");
            //Debug.log(Name[i] + "が名前登録");
            Name[i].text = PlayerName[i]; // 安全に代入
        }

        for (int i = 0; i < TotalPlayer; i++)
        {
            
            for(int j = 1; j < 11; j++)
            {
                PT[i].PL[j-1].text = gameManager.PlayerScore[i, j].ToString();
            }
            PT[i].PL[10].text = gameManager.PlayerScore[i, 0].ToString();
        }

    }




    //Point[1/*1に変数を入れる*/].text = /*1に変数を入れる*/1.ToString();
    //Point[0].text = /*1に変数を入れる*/1.ToString();





}