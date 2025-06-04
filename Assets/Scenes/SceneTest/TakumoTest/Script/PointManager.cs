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
    [SerializeField]private ScoreManager scoremanager;

    void Start()
    {
        
    }
    private void Update()
    {
        PointManeger();

    }

    void PointManeger()
    {
       
       
        for (int i = 0; i < 1/*ここにプレイヤー数を入れる*/; i++)// TPL を4行作成
        {
            //int[] score; = new int[11];
            //score[1] = scoremanager.
            PT[i/*ここにPlayernameを入れる*/].PL[1/*ここにframe数を入れる*/].text = 1/*gamemanager.playerScores[i]ここにScoreを入れる*/.ToString();
        }

       
       
    }






    //Point[1/*1に変数を入れる*/].text = /*1に変数を入れる*/1.ToString();
    //Point[0].text = /*1に変数を入れる*/1.ToString();





}