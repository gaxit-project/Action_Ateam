using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class PointPrintf : MonoBehaviour
{
    [System.Serializable]
    public class TPL
    {
        public List<Text> PL = new List<Text>(); // 1行のText
    }

    [SerializeField] private List<TPL> PT= new List<TPL>();


    public GameManager gamemanager;

    private IEnumerator Start()
    {
        gamemanager = FindFirstObjectByType<GameManager>();
        yield return new WaitForSeconds(1f);
        PointManeger();
    }


    void PointManeger()
    {
       
       
        for (int i = 0; i < 1/*ここにプレイヤー数を入れる*/; i++)// TPL を4行作成
        {
            var playerData = gamemanager.GetPlayerScoreData("Player1");
            int score = playerData.FrameScores[1];
            PT[i/*ここにPlayernameを入れる*/].PL[1/*ここにframe数を入れる*/].text = score.ToString();
        }

       
       
    }






    //Point[1/*1に変数を入れる*/].text = /*1に変数を入れる*/1.ToString();
    //Point[0].text = /*1に変数を入れる*/1.ToString();





}