using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameStarter : MonoBehaviour
{

    private PlayerBase player;
    [SerializeField] private TextMeshProUGUI countText;
    private float time = 3f;
    private bool isCountStopped = false;

    [SerializeField] private GameObject[] area = new GameObject[3];

    private GameManager gameManager;

    IEnumerator Start()
    {
        if(gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }
        countText.enabled = true;
        countText.SetText("3");
        yield return new WaitForSeconds(1f);
        gameManager.IsStart = true;
        //if (!player) player = GameObject.FindFirstObjectByType<PlayerBase>();
        //if (!player) Debug.LogError("PlayerBaseがアタッチされたオブジェクトが見つかりません！");
        //else StartCoroutine("StartMove");
    }

    void Update()
    {
        if (!isCountStopped)
        {
            time -= Time.deltaTime;
            // 小数点以下を切り捨てて整数表示
            int displayTime = Mathf.CeilToInt(time);
            countText.text = displayTime.ToString(); 
            if (time <= 0)
            {
                isCountStopped = true;
                countText.text = "GO!!";
                Invoke("Disabled", 1f);
                for(int i = 0; i <  area.Length; i++)
                {
                    if (area[i] == null)
                    {
                        Debug.LogWarning($"area[{i}] is null");
                        continue;
                    }

                    area[i].SetActive(true);
                }
            }
        }
        
        if(gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
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

    /// <summary>
    /// GameManagerのStart関数側でPlayerBaseをアタッチ
    /// </summary>
    /// <param name="player">PlayerのPlayerBase</param>
    public void SetPlayer(PlayerBase player)
    {
        this.player = player;
        StartCoroutine("StartMove");
    }

}
