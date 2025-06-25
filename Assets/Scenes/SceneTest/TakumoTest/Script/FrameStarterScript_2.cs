using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class FrameStarterScript_2 : MonoBehaviour
{
    [SerializeField] private Text FrameObjectUP;
    [SerializeField] private Transform WarpObject;

    [SerializeField] private float moveDuration = 1.5f;     // 移動にかける秒数
    [SerializeField] private float scaleDuration = 1.5f;    // 縮小にかける秒数

    public bool isFinshed = false;
    public GameManager gameManager;
    private Player player;

    private Vector3 startPos;
    private Vector3 initialScale;
    private Vector3 targetScale = new Vector3(0.9f, 0.9f, 0.9f); // 最小スケール（消えない程度）

    private Vector3 targetPos;
    private float elapsedTime = 0f;
    private bool isMoving = false;

    /// <summary>
    /// FrameObjectUP;は移動したいobjectである。
    ///  WarpObject;は FrameObjectUPの移動先である。
    ///  
    ///  Durationは移動完了までの速度である。
    ///  
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

        if (WarpObject != null)
        {
            targetPos = WarpObject.position;
        }

        StartCoroutine(DelayedStart());
        AudioManager.Instance.PlaySound(7);
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(1f);
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving || FrameObjectUP == null || WarpObject == null || isFinshed) return;//ヌルチェック

        if(player == null) player = GameObject.FindFirstObjectByType<Player>();

        elapsedTime += Time.deltaTime;

        float moveT = Mathf.Clamp01(elapsedTime / moveDuration);
        float scaleT = Mathf.Clamp01(elapsedTime / scaleDuration);

        // 移動と縮小を時間ベースで補間
        FrameObjectUP.transform.position = Vector3.Lerp(startPos, targetPos, moveT);
        FrameObjectUP.transform.localScale = Vector3.Lerp(initialScale, targetScale, scaleT);

        if (moveT >= 1f && scaleT >= 1f)
        {
            isFinshed = true;
            player.ChangeArrowMode();
            Debug.Log("目的地に到達＆縮小完了！");
            this.enabled = false;
        }
    }

    public void FrameObjectUPFalse()
    {
        FrameObjectUP.gameObject.SetActive(false); // 数字を非表示にする
    }
}
