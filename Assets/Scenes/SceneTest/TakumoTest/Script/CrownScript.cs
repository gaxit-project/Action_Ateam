using System.Collections.Generic;
using NPC.StateAI;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.TextCore;
using static Player;

public class CrownScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private GameManager gameManager;
    //Scoreの取得が必要なスクリプトを持ってくる

    public void GetScore()
    {
        gameManager = FindFirstObjectByType<GameManager>();




        int player = gameManager.NumHumanPlayers, bot = gameManager.NumBots;
        int[] scores = new int[player + bot];
        int i = 0, j = 0, isNum = 0;



        /*ScoreをPlayerーbotの順で順番に取得する*/
        while (i < player)
        {
            var playerdata = gameManager.GetPlayerScoreData("Player" + (i+1));
            scores[i] = playerdata.GetTotalScore();
            i++;
        }

        while (j < bot)
        {
            var botdata = gameManager.GetPlayerScoreData("Bot" + (isNum + 1));
            scores[i] = botdata.GetTotalScore();/*botjのScore*/ ;
            isNum++;
            j++;
        }






        // 最大スコアのインデックスを取得
        int maxIndex = 0;
        for (i = 1; i < player + bot; i++)
        {

            if (scores[i] > scores[maxIndex])
            {
                maxIndex = i;
            }
        }
        string ID;
        bool isPlayer;
        if (maxIndex + 1 <= player)
        {
            ID = ("Player" + (maxIndex + 1));
            isPlayer = true;
        }
        else
        {
            ID = ("bot" + (maxIndex + 1 - player));
            isPlayer = false;
        }


        // 該当するClownだけを有効化,GetPlayerID()
        GameObject[] chara;
        if (isPlayer)
        {
            chara = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject Aris in chara)
            {
                Player player1 = Aris.GetComponent<Player>();
                if (player1 != null && player1.GetPlayerID() == ID)
                {
                    player1.Crowned();
                    break;
                }
            }
        }
        else
        {
            chara = GameObject.FindGameObjectsWithTag("NPC");
            foreach (GameObject Aris in chara)
            {
                EnemyAI player1 = Aris.GetComponent<EnemyAI>();
                if (player1 != null && player1.GetPlayerID() == ID)
                {
                    player1.Crowned();
                    break;
                }
            }

        }

    }

}