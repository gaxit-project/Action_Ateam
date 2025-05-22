using TMPro;
using UnityEngine;

public class GameStarter : SingletonMonoBehaviour<GameStarter>
{

    private PlayerBase player;
    [SerializeField] private TextMeshProUGUI countText;
    private float time = 3f;
    private bool isCountStoped = false;

    void Start()
    {
        countText.enabled = true; 
        countText.SetText("3");
        if (!player) player = GameObject.FindFirstObjectByType<PlayerBase>();
        if (!player) Debug.LogError("PlayerBaseがアタッチされたオブジェクトが見つかりません！");
        else StartCoroutine("StartMove");
    }

    void Update()
    {
        if (!isCountStoped)
        {
            time -= Time.deltaTime;
            // 小数点以下を切り捨てて整数表示
            int displayTime = Mathf.CeilToInt(time);
            countText.text = displayTime.ToString();
        }
        if(time <= 0)
        {
            isCountStoped = true;
            countText.text = "GO!!";
            Invoke("Disabled", 1f);
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

}
