/*using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrintText : MonoBehaviour
{
    public GameObject Score_obj = null; //Textオブジェクト
    [SerializeField]private GameManager gameManager;
    int score = 0;

    private void Awake()
    {
        //GameManagerがnullなら探す
        if(gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }
    }
    void Update()
    {
        //GameManagerでスコア計算
        score = gameManager.ScoreText();       

        TextMeshProUGUI score_text = Score_obj.GetComponent<TextMeshProUGUI>();      
        score_text.text = "Score:" + score.ToString();
        
    }
}
*/