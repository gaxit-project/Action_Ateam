using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private float displayScoreTime = 3;

    public PinManager pinManager;
    public ResetArea resetArea;

    public GameObject score_object = null;
    public int totalScore;

    private void Start()
    {
    }

    private void Update()
    {
        
    }

    public void currentFrameResult()
    {
        ScoreText();
        StartCoroutine(DelayAndResetCoroutine());
    }

    public void ScoreText()
    {
        string strText;
        Text score_text = score_object.GetComponent<Text>();
        strText = pinManager.GetKnockedDownPinCount().ToString("0");
        score_text.text = strText;
    }

    private IEnumerator DelayAndResetCoroutine()
    {
        yield return new WaitForSeconds(displayScoreTime);
        resetArea.ResetGame();
    }
}
