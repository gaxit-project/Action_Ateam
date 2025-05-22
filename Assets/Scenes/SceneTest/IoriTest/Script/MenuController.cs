using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Button pushToStartButton;
    public RectTransform startButton;
    public RectTransform settingsButton;
    public RectTransform quitButton;
    public RectTransform arrow;

    private RectTransform[] menuButtons;
    private Button[] buttonComponents;
    private int currentIndex = 0;

    private float inputCooldown = 0.3f;
    private float lastInputTime = 0f;

    void Start()
    {
        menuButtons = new RectTransform[] { startButton, settingsButton, quitButton }; //menuButton配列初期化
        buttonComponents = new Button[]
        {
            startButton.GetComponent<Button>(),
            settingsButton.GetComponent<Button>(),
            quitButton.GetComponent<Button>()
        };

        foreach (var btn in menuButtons)
        {
            btn.gameObject.SetActive(false);
        }

        arrow.gameObject.SetActive(false);
    }

    /// <summary>
    /// PushToStartが押されたとき
    /// </summary>
    public void OnPushToStart()
    {
        pushToStartButton.gameObject.SetActive(false);
        StartCoroutine(ShowMenuWithSlide());
    }

    /// <summary>
    /// ボタンを画面外でアクティブにする
    /// </summary>
    /// <returns>コルーチン「SmoothSlide」開始</returns>
    IEnumerator ShowMenuWithSlide()
    {
        Vector2[] targetPositions = new Vector2[menuButtons.Length];

        for (int i = 0; i < menuButtons.Length; i++)
        {
            targetPositions[i] = menuButtons[i].anchoredPosition;
            menuButtons[i].anchoredPosition += new Vector2(1500, 0); // 右画面外
            menuButtons[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < menuButtons.Length; i++)
        {
            yield return StartCoroutine(SmoothSlide(menuButtons[i], targetPositions[i]));
        }

        arrow.gameObject.SetActive(true);
        UpdateArrowPosition();
    }

    /// <summary>
    /// ボタンが画面外からスライド
    /// </summary>
    /// <param name="btn">ボタンの位置</param>
    /// <param name="target">ボタンの最終位置</param>
    /// <returns></returns>
    IEnumerator SmoothSlide(RectTransform btn, Vector2 target)
    {
        float duration = 0.5f;
        float elapsed = 0.2f;
        Vector2 start = btn.anchoredPosition;

        while (elapsed < duration)
        {
            btn.anchoredPosition = Vector2.Lerp(start, target, Mathf.SmoothStep(0f, 1f, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        btn.anchoredPosition = target;
    }

    void Update()
    {
        if (!arrow.gameObject.activeSelf) return; //PushToStartが押されていない場合

        // 入力クールダウンチェック
        if (Time.time - lastInputTime > inputCooldown)
        {
            float vertical = Input.GetAxis("Vertical");

            // 上方向
            if (Input.GetKeyDown(KeyCode.UpArrow) || vertical > 0.5f)
            {
                currentIndex = (currentIndex + menuButtons.Length - 1) % menuButtons.Length;
                UpdateArrowPosition();
                lastInputTime = Time.time;
            }
            // 下方向
            else if (Input.GetKeyDown(KeyCode.DownArrow) || vertical < -0.5f)
            {
                currentIndex = (currentIndex + 1) % menuButtons.Length;
                UpdateArrowPosition();
                lastInputTime = Time.time;
            }
        }

        // 決定（EnterキーまたはAボタン）
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            buttonComponents[currentIndex].onClick.Invoke();
        }
    }

    /// <summary>
    /// カーソルの位置変更
    /// </summary>
    void UpdateArrowPosition()
    {
        // 全てのボタンのスケールをリセット
        for (int i = 0; i < menuButtons.Length; i++)
        {
            menuButtons[i].localScale = new Vector3(3, 3, 1);
        }

        // 選択中のボタンを強調
        RectTransform target = menuButtons[currentIndex];
        arrow.anchoredPosition = new Vector2(target.anchoredPosition.x - 500, target.anchoredPosition.y);
        AudioManager.Instance.PlaySound(0);        
        target.localScale = new Vector3(3.6f, 3.6f, 1);

    }
}
