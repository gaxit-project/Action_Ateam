using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FrameStarterScript : MonoBehaviour
{
    [SerializeField] private Text FrameObjectUP;//ここに上に表示したいテキスト
    [SerializeField] private Text FrameObjectLeft;//ここに右に表示したいテキスト
    [SerializeField] private Image FrameObjectTV;//これはTVpng
    [SerializeField] private float speed = 50f;//これは移動スピード
    
    public GameManager gameManager;//げーまね読んだ

    private float timer = 0f;
    private Vector2 startPos;
    private bool isReturning = false;
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        FrameObjectUP.text = gameManager.Num_NowFrame.ToString() + "f";
        FrameObjectLeft.text = gameManager.Num_NowFrame.ToString() + "f";

        if (FrameObjectTV != null)//初手の位置を把握
        {
            startPos = FrameObjectTV.rectTransform.anchoredPosition;
        }
    }

    void Update()
    {

        //while () { 
        if (FrameObjectTV == null) return;

        timer += Time.deltaTime;

        if (timer < 1f)//１秒間は動く
        {
            // 0～1秒: 上に移動
            FrameObjectTV.rectTransform.anchoredPosition += Vector2.up * speed * Time.deltaTime;
        }
        else if (timer >= 2f && !isReturning)
        {
            // 2秒経過したら戻るモード開始
            isReturning = true;
        }

            if (isReturning)
            {
                // 元の位置へ線形補間で戻る（1秒間かけて）
                float t = (timer - 2f) / 1f; // 0〜1 に正規化
                FrameObjectTV.rectTransform.anchoredPosition = Vector2.Lerp(
                    startPos + Vector2.up * speed, // 上がった先
                    startPos,                      // 元の位置
                    t
                );
                
                // 戻り終わったら止める
                if (t >= 1f)
                {
                    //ここに特定のGameStarterから持って来る
                    isReturning = false;
                    this.enabled = false;
                }
                
            //}


        }


    }

    public void FrameObjectLeftFalse(){
        FrameObjectLeft.gameObject.SetActive(false);//右上に出てる数字を消す
    }

}
