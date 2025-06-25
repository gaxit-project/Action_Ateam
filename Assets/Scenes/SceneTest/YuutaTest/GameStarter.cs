using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameStarter : MonoBehaviour
{

    private Player player;
    [SerializeField] private TextMeshProUGUI countText;
    private float time = 3f;
    private bool isCountStopped = false;
    private int count = 3;
    private bool isCountStarted = false;

    [SerializeField] private GameObject[] area = new GameObject[3];

    private GameManager gameManager;
    private FrameStarterScript_2 frameStarterScript_2;

    IEnumerator Start()
    {
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }
        countText.enabled = true;
        countText.SetText("");
        yield return new WaitForSeconds(1f);
        gameManager.IsStart = true;
        frameStarterScript_2 = GameObject.FindFirstObjectByType<FrameStarterScript_2>();
        //if (!player) player = GameObject.FindFirstObjectByType<PlayerBase>();
        //if (!player) Debug.LogError("PlayerBaseがアタッチされたオブジェクトが見つかりません！");
        //else StartCoroutine("StartMove");
    }

    void Update()
    {

        if (frameStarterScript_2 != null && frameStarterScript_2.isFinshed/*frameStarterScriptがfalseの時*/)
        {//frameStarterScriptがfalseの時真となる
            if (!isCountStopped)
            {
                time -= Time.deltaTime;
                // 小数点以下を切り捨てて整数表示
                int displayTime = Mathf.CeilToInt(time);
                countText.text = displayTime.ToString();
                StartCount();
                if (time <= 0)
                {
                    isCountStopped = true;
                    countText.text = "GO!!";
                    AudioManager.Instance.PlaySound(6);
                    Invoke("Disabled", 0.5f);
                    for (int i = 0; i < area.Length; i++)
                    {
                        if (area[i] == null)
                        {
                            Debug.LogWarning($"area[{i}] is null");
                            continue;
                        }

                        area[i].SetActive(true);
                    }
                    player.ChangeArrowMode();
                }
            }

            if (gameManager == null)
            {
                gameManager = FindFirstObjectByType<GameManager>();
            }
        }
    }

    private void StartMove()
    {
        player.Invoke(nameof(player.StartMove), 3f);
    }

    private void Disabled()
    {
        countText.enabled = false;
    }

    public bool IsCountStopped()
    {
        return isCountStopped;
    }

    private void StartCount()
    {
        if(!isCountStarted)
        {
            isCountStarted = true;
            Invoke("PlayCountSound", 0f);
        }
    }

    private void PlayCountSound()
    {
        if (count > 0)
        {
            count--;
            AudioManager.Instance.PlaySound(5);
            Invoke("PlayCountSound", 1f);
        }
    }

    /// <summary>
    /// GameManagerのStart関数側でPlayerBaseをアタッチ
    /// </summary>
    /// <param name="player">PlayerのPlayerBase</param>
    public void SetPlayer(Player player)
    {
        this.player = player;
        StartCoroutine("StartMove");
    }

}
