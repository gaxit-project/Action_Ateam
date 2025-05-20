using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour
{
    public UnityEngine.UI.Button pushToStartButton;
    public RectTransform startButton;
    public RectTransform settingsButton;
    public RectTransform quitButton;
    public RectTransform arrow;

    private RectTransform[] menuButtons;
    private int currentIndex = 0;

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
        StartCoroutine(ShowMenuWithSlide());
    }

    IEnumerator ShowMenuWithSlide()
    {
        Vector2[] targetPositions = new Vector2[menuButtons.Length];

        for (int i = 0; i < menuButtons.Length; i++)
        {
            targetPositions[i] = menuButtons[i].anchoredPosition;
            // ‰E‰æ–ÊŠO‚ÖˆÚ“®
            menuButtons[i].anchoredPosition += new Vector2(800, 0);
            menuButtons[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < menuButtons.Length; i++)
        {
            yield return StartCoroutine(SmoothSlide(menuButtons[i], targetPositions[i]));
        }

        arrow.gameObject.SetActive(true);
        UpdateArrowPosition();
    }

    IEnumerator SmoothSlide(RectTransform btn, Vector2 target)
    {
        float speed = 50f;

        while (Vector2.Distance(btn.anchoredPosition, target) > 0.1f)
        {
            btn.anchoredPosition = Vector2.MoveTowards(btn.anchoredPosition, target, speed);
            yield return null;
        }

        btn.anchoredPosition = target;
    }

    void Update()
    {
        if (!arrow.gameObject.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex = (currentIndex + menuButtons.Length - 1) % menuButtons.Length;
            UpdateArrowPosition();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex = (currentIndex + 1) % menuButtons.Length;
            UpdateArrowPosition();
        }
    }

    void UpdateArrowPosition()
    {
        RectTransform target = menuButtons[currentIndex];
        arrow.anchoredPosition = new Vector2(target.anchoredPosition.x - 500, target.anchoredPosition.y);
    }
}
