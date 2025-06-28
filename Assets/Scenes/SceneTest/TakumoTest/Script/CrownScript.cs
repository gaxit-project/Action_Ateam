using UnityEngine;
using UnityEngine.TextCore;
using static Player;

public class CrownScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private GameObject Crown0;//Player0のClown
    [SerializeField] private GameObject Crown1;//Player1のClown
    [SerializeField] private GameObject Crown2;//Player2のClown
    [SerializeField] private GameObject Crown3;//Player3のClown

    private GameManager gameManager;
    //Scoreの取得が必要なスクリプトを持ってくる

    void Start()
    {
       gameManager = FindFirstObjectByType<GameManager>();
        GameObject[] clowns = new GameObject[] { Crown0, Crown1, Crown2, Crown3 };


        
        int player = gameManager.NumHumanPlayers, bot = gameManager.NumBots;
        int[] scores = new int[player + bot];
        int i=0, j=0, isNum = 0;


        
        /*ScoreをPlayerーbotの順で順番に取得する*/    
        while(i < player )
        {
            var playerdata = gameManager.GetPlayerScoreData("Player" + i+1);
            scores[i] = playerdata.GetTotalScore();
            i++;
        }

        while  (j < bot)
        {
            var botdata = gameManager.GetPlayerScoreData("Bot" + (isNum + 1));
            scores[i] = botdata.GetTotalScore();/*botjのScore*/ ;
            isNum++;
            j++;
        }






        // 最大スコアのインデックスを取得
        int maxIndex = 0;
        for (i = 1; i <player+bot; i++)
        {
            
            if (scores[i] > scores[maxIndex])
            {
                maxIndex = i;
            }
        }

        // 該当するClownだけを有効化
        clowns[maxIndex].SetActive(true);
    }
}