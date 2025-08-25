using NPC.StateAI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.TextCore;
using UnityEngine.UIElements;
using static Player;
using static Player2;

public class CrownScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private GameManager gameManager;
    //Scoreの取得が必要なスクリプトを持ってくる

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void CrownMove()
    {
        if(gameManager == null)
        {
            gameManager = GameManager.Instance;     
        }
        int player = gameManager.NumHumanPlayers, bot = gameManager.NumBots;
        int[] scores = new int[player + bot];
        int i = 0, j = 0, isNum = 0;



        /*ScoreをPlayerーbotの順で順番に取得する*/
        while (i<player)
        {
            var playerdata = gameManager.GetPlayerScoreData("Player" + (i + 1));
            if (playerdata != null)
            {
                scores[i] = playerdata.GetTotalScore();
            }
            else
            {
                Debug.LogError("ERROR");
            }
        i++;
        }

        while (j < bot)
        {
            var botdata = gameManager.GetPlayerScoreData("Bot" + (isNum + 1));
            scores[i++] = botdata.GetTotalScore();/*botjのScore*/ ;
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
            ID = ("Bot" + (maxIndex + 1 - player));
            Debug.Log("ID:Bot" + (maxIndex + 1 - player));
            isPlayer = false;
        }


        // 該当するcrownだけを有効化,GetPlayerID()
        GameObject[] chara;
        if (scores[maxIndex] > 0)
        {
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
                    Debug.Log(player1.GetPlayerID());
                    if (player1 != null && player1.GetPlayerID() == ID)
                    {
                        player1.Crowned();
                        break;
                    }
                    //Debug.LogError("1位のNPCが見つかりません!");
                }
            }
        }       
    }
}