using System.Collections;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private float displayScoreTime = 3;
    [SerializeField] int Num_NowFrame=1;
    

    public PinManager pinManager;
    public ResetArea resetArea;

    public GameObject score_object = null;
    public int totalScore;


    private int[] NowFramePoint_0 = new int[11];
    private int[] NowFramePoint_1 = new int[11];
    private int[] NowFramePoint_2 = new int[11];
    private int[] NowFramePoint_3 = new int[11];
    int pp0,pp1,pp2,pp3;//a-dは仮置き,スコアを表す


    public void ResetStart()
    {
        Debug.Log("RESET");
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 144;
        pinManager = FindFirstObjectByType<PinManager>();
        resetArea = FindFirstObjectByType<ResetArea>();
        score_object = GameObject.Find("Canvas/Text (Legacy)");
    }

    private void Update()
    {
        if (false/*resetArea.OnTriggerEnter*/)
        {
            FrameSaveSystem();
        }
        if (pinManager == null || resetArea == null || score_object == null)
        {
            ResetStart();
        }
    }

    public void FrameSaveSystem()
    {
        
        if (true) {// かつPlayer0であれば
            pp0 = pinManager.GetKnockedDownPinCount();

    NowFramePoint_0[Num_NowFrame] = pp0;//今回の得点
        NowFramePoint_0[0] += NowFramePoint_0[Num_NowFrame];//合計点
        }//if文の終わり



        Num_NowFrame++;
        resetArea.ResetGame();
        Debug.Log("現在 +Num_NowFrame+ frameになりました");
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
