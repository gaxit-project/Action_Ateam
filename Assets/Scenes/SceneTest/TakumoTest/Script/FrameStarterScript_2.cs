using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections; // ← Coroutineに必要

public class FrameStarterScript_2 : MonoBehaviour
{
    [SerializeField] private Text FrameObjectUP;
    [SerializeField] private Transform WarpObject;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float scaleSpeed = 1f;
    public bool isFinshed = false;

    public GameManager gameManager;

    private Vector3 startPos;
    private Vector3 initialScale;
    private Vector3 targetScale = Vector3.zero;//FrameObjectUPが消えるまで縮小

    private bool isMoving = false; // ← 1秒後に true にする

    /// <summary>
    /// FrameObjectUP;は移動したいobjectである。
    ///  WarpObject;は FrameObjectUPの移動先である。
    ///  speedは FrameObjectUP;の移動速度である。
    ///  scaleSpeedはFrameObjectUP;が小さくなるまでの速度である
    ///  
    /// 基本的にFrameObjectUPーWarpObjectの移動をspeed系で調整する。
    /// FrameObjectUPがWarpObjectと重なったらこのスクリプトは止まりカウントダウンが開始する。
    /// </summary>



    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        FrameObjectUP.text = gameManager.Num_NowFrame.ToString() + "f";

        if (FrameObjectUP != null)
        {
            startPos = FrameObjectUP.transform.position;
            initialScale = FrameObjectUP.transform.localScale;
        }

        // 1秒待ってから移動開始
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(1f);
        isMoving = true;//一秒立ったからここでフラグを立てる
    }

    void Update()
    {
        if (!isMoving || FrameObjectUP == null || WarpObject == null || isFinshed) return;

        Vector3 currentPos = FrameObjectUP.transform.position;
        Vector3 currentScale = FrameObjectUP.transform.localScale;
        Vector3 targetPos = WarpObject.position;

        float moveStep = speed * Time.deltaTime;
        FrameObjectUP.transform.position = Vector3.MoveTowards(currentPos, targetPos, moveStep);

        float scaleStep = scaleSpeed * Time.deltaTime;
        FrameObjectUP.transform.localScale = Vector3.MoveTowards(currentScale, targetScale, scaleStep);

        if (Vector3.Distance(FrameObjectUP.transform.position, targetPos) < 0.01f)
        {
            isFinshed = true;
            Debug.Log("目的地に到達＆縮小完了！");
            this.enabled = false;
        }
    }
    public void FrameObjectUPFalse()
    {
        FrameObjectUP.gameObject.SetActive(false);//右上に出てる数字を消す
    }

}


