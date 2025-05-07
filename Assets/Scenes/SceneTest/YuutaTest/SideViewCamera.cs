using UnityEngine;

public class SideViewCamera: SingletonMonoBehaviour<SideViewCamera> //今はプレイヤーがx方向にのみ動く場合を考える
{
    //各オブジェクトの座標取得用
    private GameObject[] players;
    private Vector3[] playerPositions;
    [SerializeField]private GameObject startPoint;
    [SerializeField] private string startPointName = "StartPoint";
    [SerializeField]private GameObject goalPoint;
    [SerializeField] private string goalPointName = "GoalPoint";

    //カメラ位置計算用
    private float closer;
    private float distance;
    private Vector3 topPlayerPosition;
    private Vector3 lastPlayerPosition;

    void Start()
    {
        //スタート地点とゴール地点の座標取得
        if(startPoint == null && startPointName != null)
        {
            startPoint = GameObject.Find(startPointName);
        }
        else if(startPoint == null)
        {
            Debug.LogError("スタート地点が存在しないまたは名前が違います!");
        }
        if (goalPoint == null && goalPointName != null)
        {
            goalPoint = GameObject.Find(goalPointName);
        }
        else if(goalPoint == null)
        {
            Debug.LogError("ゴール地点が存在しないまたは名前が違います!");
        }
        //プレイヤーが1人以下の場合にエラー表示
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length < 2) Debug.LogError("プレイヤーの数は2人以上にしてください!");

    }

    
    void Update()
    {
        //プレイヤーの座標取得
        players = GameObject.FindGameObjectsWithTag("Player");
        if (!(players.Length < 2))
        {
            playerPositions = new Vector3[players.Length];
            for (int i = 0; i < players.Length; i++)
            {
                playerPositions[i] = players[i].transform.position;
            }
        }

        if (startPoint != null && goalPoint != null)
        {
            //1位と最下位の位置を取得
            topPlayerPosition = playerPositions[0];
            lastPlayerPosition = playerPositions[0];
            foreach (Vector3 playerPosition in playerPositions)
            {
                if (Vector3.Distance(playerPosition, goalPoint.transform.position) < Vector3.Distance(topPlayerPosition, goalPoint.transform.position))
                {
                    topPlayerPosition = playerPosition;
                }
                if (Vector3.Distance(playerPosition, startPoint.transform.position) < Vector3.Distance(lastPlayerPosition, startPoint.transform.position))
                {
                    lastPlayerPosition = playerPosition;
                }
            }
            //距離の取得(カメラ位置計算用)
            float distanceToTop = topPlayerPosition.x - lastPlayerPosition.x;
            float distanceToGoal = goalPoint.transform.position.x - lastPlayerPosition.x;
            closer = (distanceToTop < distanceToGoal) ? topPlayerPosition.x : goalPoint.transform.position.x; //最下位から見て1位かゴールどちらが近いか
            distance = closer - lastPlayerPosition.x;
        }
    }

    private void FixedUpdate()
    {
        float cameraX = Mathf.Clamp(((distance > 50f) ? closer - 25f : closer - distance / 2f), 0f, ((distance > 50f) ? closer - 25f : 25f));
        float cameraY = Mathf.Clamp(10f + distance / 10f, 10f, 15f);
        float cameraZ = -Mathf.Clamp(15f + distance * 3f / 10f, 15f, 30f);

        transform.position = new Vector3(cameraX, cameraY, cameraZ);
    }
}
