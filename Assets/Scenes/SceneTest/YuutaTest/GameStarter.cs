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

    private GameManager gameManager;

    void Start()
    {
        countText.enabled = true; 
        countText.SetText("3");
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
        }
        if(time <= 0)
        {
            isCountStopped = true;
            countText.text = "GO!!";
            Invoke("Disabled", 1f);
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

    /// <summary>
    /// Mainに入ったときにプレイヤーを作成
    /// </summary>
    public void SetUpPlayers()
    {
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
            if (gameManager == null)
            {
                Debug.LogError("GameManager.Instance が null です");
                return;
            }
        }

        if (gameManager.players == null)
            gameManager.players = new List<PlayerBase>();

        if (gameManager.playerScores == null)
            gameManager.playerScores = new List<PlayerScoreData>();

        if (!gameManager.IsStart)
        {
            Debug.LogWarning("クリアされたよ");
            gameManager.players.Clear();
            gameManager.playerScores.Clear();
        }

        //合計人数
        int totalPlayers = gameManager.NumHumanPlayers + gameManager.NumBots;

        //スポーン地点をリストに保存
        List<Vector3> spawnPositions = new List<Vector3>();
        for(int i = 0; i < totalPlayers; i++)
        {
            //現在はx座標を75fずつ左にずらしている状態
            spawnPositions.Add(new Vector3(i * -75f, 0f, 0f)); //ここをいじって変えてください
        }

        spawnPositions = spawnPositions.OrderBy(x => Random.value).ToList(); //スポーン場所をランダムに

        int spawnIndex = 0; //人数
        //Player
        for (int i = 0; i < gameManager.NumHumanPlayers; i++)
        {
            var playerobj = Instantiate(gameManager._playerPrefab, spawnPositions[spawnIndex++], Quaternion.identity);
            var player = playerobj.GetComponent<PlayerBase>();
            if(gameManager.IsStart == false)
            {
                player.Init($"Player{i + 1}", false);
                gameManager.players.Add(player);
                gameManager.playerScores.Add(new PlayerScoreData($"Player{i + 1}", false));
            }
            var cam = FindFirstObjectByType<CameraController>();
            if (cam == null) Debug.LogError("nullだよ");
            cam.SetTargetPlayer(player); // Instantiate後に必ず設定！
            var starter = FindFirstObjectByType<GameStarter>();
            if (starter == null) Debug.LogError("starternull");
            starter.SetPlayer(player);

        }

        //Bot
        for (int i = 0; i < gameManager.NumBots; i++)
        {
            var botobj = Instantiate(gameManager._botPrefab, spawnPositions[spawnIndex++], Quaternion.identity);
            var bot = botobj.GetComponent<PlayerBase>();
            if(gameManager.IsStart == false)
            {
                bot.Init($"Bot{i + 1}", true);
                gameManager.players.Add(bot);
                gameManager.playerScores.Add(new PlayerScoreData($"Bot{i + 1}", true));
            }

        }
    }
}
