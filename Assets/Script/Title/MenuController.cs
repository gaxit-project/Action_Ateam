using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    public Button pushToStartButton;
    public RectTransform startButton;
    public RectTransform settingsButton;
    public RectTransform quitButton;
    public RectTransform arrow;

    private RectTransform[] menuButtons;

    private GameObject LastSelected = null;

    void Start()
    {
        menuButtons = new RectTransform[] { startButton, settingsButton, quitButton };

        foreach (var btn in menuButtons)
        {
            btn.gameObject.SetActive(false);
        }

        arrow.gameObject.SetActive(false);
    }

    public void OnPushToStart()
    {
        pushToStartButton.gameObject.SetActive(false);
        AudioManager.Instance.PlaySound(3);
        StartCoroutine(ShowMenuWithSlide());
    }

    IEnumerator ShowMenuWithSlide()
    {
        Vector2[] targetPositions = new Vector2[menuButtons.Length];

        for (int i = 0; i < menuButtons.Length; i++)
        {
            targetPositions[i] = menuButtons[i].anchoredPosition;
            menuButtons[i].anchoredPosition += new Vector2(1500, 0);
            menuButtons[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < menuButtons.Length; i++)
        {
            yield return StartCoroutine(SmoothSlide(menuButtons[i], targetPositions[i]));
        }

        arrow.gameObject.SetActive(true);

        // 最初の選択を明示的に設定
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
    }

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
        if (!arrow.gameObject.activeSelf) return;

        UpdateArrowPosition();
    }

    void UpdateArrowPosition()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected == null || selected.GetComponent<RectTransform>() == null)
            return;

        // 前回と同じ選択なら何もしない
        if (selected == LastSelected) return;

        // 選択が変わったときだけサウンド再生
        AudioManager.Instance.PlaySound(0);
        LastSelected = selected;

        RectTransform selectedRect = selected.GetComponent<RectTransform>();

        // 矢印の位置を更新
        Vector2 offset = new Vector2(-400f, 0f);
        arrow.anchoredPosition = selectedRect.anchoredPosition + offset;

        // スケールの強調表示
        foreach (RectTransform btn in menuButtons)
        {
            btn.localScale = new Vector3(3, 3, 1);
        }

        selectedRect.localScale = new Vector3(3.6f, 3.6f, 1);
    }

}
