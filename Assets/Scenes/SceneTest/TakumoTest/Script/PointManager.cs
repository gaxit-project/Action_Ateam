using UnityEngine;
using UnityEngine.UI;

public class TextSetter : MonoBehaviour
{
    [SerializeField] private Text[] Point = new Text[11]; // ← インスペクターでアサイン
    public GameManager gamemanager;


    public 
    void Start()
    {
        
    }
    private void Update()
    {
        PointManeger();

    }

    void PointManeger()
    {
        Point[1/*1に変数を入れる*/].text = /*1に変数を入れる*/1.ToString();
        Point[0].text = /*1に変数を入れる*/1.ToString();

    }



}