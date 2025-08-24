using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

public class SelectArea : MonoBehaviour
{
    public GameObject[] spawnObjects;
    private int currentObjectIndex = 0;
    [System.Serializable]
    public class SpawnStages
    {
        public List<GameObject> Stages = new List<GameObject>();
    }
    public SpawnStages[] spawnAreas;
    public Material highlightMaterial;
    public Material defaultMaterial;
    public Material occupiedMaterial;
    private int currentAreaIndex = 0;
    private PlayerInputActions inputActions;
    private float lastInputTime = 0f;
    private float inputCooldown = 0.2f;
    private float lastSpawnTime = 0f;
    private float spawnCooldown = 0.5f;
    private bool[] areaOccupied;
    public bool isAllAreasFilled = false;
    [SerializeField] private List<GameObject> spawnedObjects;
    private List<GameObject> availableSpawnObjects;

    private GameManager gameManager;
    private int StageNumber;

    private bool isSelect = true;

    [SerializeField] private int PlayerNumber = 0;
    void Start()
    {
        PlayerNumber = 0;

        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Spawn.performed += OnSpawnPerformed;

        gameManager = GameManager.Instance;

        //エリア初期化
        areaOccupied = new bool[spawnAreas[StageNumber].Stages.Count];
        spawnedObjects = new List<GameObject>(new GameObject[spawnAreas[StageNumber].Stages.Count]);
        spawnObjects = new GameObject[spawnAreas[StageNumber].Stages.Count];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (spawnAreas[i].Stages[j] != null)
                {
                    areaOccupied[j] = false;
                    spawnedObjects.Add(null);
                    spawnAreas[i].Stages[j].GetComponent<Renderer>().material = defaultMaterial;
                }
            }
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        //入力重複対策
        if (Time.time - lastInputTime < inputCooldown) return;

        //スティック入力
        float horizontalInput = context.ReadValue<Vector2>().x;
        if (Mathf.Abs(horizontalInput) > 0.5f)
        {
            int previousIndex = currentAreaIndex;
            //左入力
            if (horizontalInput < 0)
            {
                do
                {
                    currentAreaIndex = (currentAreaIndex - 1 + spawnAreas[StageNumber].Stages.Count) % spawnAreas[StageNumber].Stages.Count;
                } while (currentAreaIndex < areaOccupied.Length && (spawnAreas[StageNumber].Stages[currentAreaIndex] == null || areaOccupied[currentAreaIndex]) && currentAreaIndex != previousIndex);
            }
            //右入力
            else if (horizontalInput > 0)
            {
                do
                {
                    currentAreaIndex = (currentAreaIndex + 1) % spawnAreas[StageNumber].Stages.Count;
                } while (currentAreaIndex < areaOccupied.Length && (spawnAreas[StageNumber].Stages[currentAreaIndex] == null || areaOccupied[currentAreaIndex]) && currentAreaIndex != previousIndex);
            }

            if (currentAreaIndex < spawnAreas[StageNumber].Stages.Count && currentAreaIndex < areaOccupied.Length)
            {
                if (spawnAreas[StageNumber].Stages[currentAreaIndex] != null && !areaOccupied[currentAreaIndex])
                {
                    SelectAreas(currentAreaIndex);
                }
            }
            lastInputTime = Time.time; //入力時刻更新
        }
    }

    // プレイヤーの生成処理
    private void OnSpawnPerformed(InputAction.CallbackContext context)
    {
        if (Time.time - lastSpawnTime < spawnCooldown || availableSpawnObjects.Count == 0 || areaOccupied[currentAreaIndex]) return;

        // リストの先頭にあるオブジェクトを取得
        GameObject nextObject = availableSpawnObjects[0];

        // プレイヤーの場合は、選択されたエリアに生成
        Vector3 spawnPosition = GetSelectedSpawnPosition();
        nextObject.transform.position = spawnPosition + new Vector3(0f, 1f, 0f);
        spawnedObjects[currentAreaIndex] = nextObject;

        // オブジェクトをリストから削除
        availableSpawnObjects.RemoveAt(0);

        // エリアを占有済みにする
        Destroy(spawnAreas[StageNumber].Stages[currentAreaIndex]);
        spawnAreas[StageNumber].Stages[currentAreaIndex] = null;
        areaOccupied[currentAreaIndex] = true;

        CheckAllAreasFilled();

        lastSpawnTime = Time.time;

        isSelect = false;

        CameraController cameraController = FindFirstObjectByType<CameraController>();
        cameraController.throwingCameraPosition = gameManager.StartPoint - new Vector3(0f, 0f, 5f);

        // 生成が完了した後に、次のオブジェクトがCPUかどうかを確認する
        ListPlayer();
    }

    // CPUの自動生成処理（ランダムなエリアを選択）
    // CPUの自動生成処理（ランダムなエリアを選択）
    private void SpawnCpuObject(GameObject objectToSpawn)
    {
        // 現在空いているエリアのインデックスをリストに格納
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < areaOccupied.Length; i++)
        {
            if (spawnAreas[StageNumber].Stages[i] != null && !areaOccupied[i])
            {
                availableIndices.Add(i);
            }
        }

        // 空いているエリアがあれば、その中からランダムに1つ選択
        if (availableIndices.Count > 0)
        {
            int randomIndex = availableIndices[Random.Range(0, availableIndices.Count)];

            // オブジェクトを生成
            Vector3 spawnPosition = spawnAreas[StageNumber].Stages[randomIndex].transform.position + new Vector3(0f, spawnAreas[StageNumber].Stages[randomIndex].transform.localScale.y / 2, 0);
            objectToSpawn.transform.position = spawnPosition + new Vector3(0f, 1f, 0f);
            spawnedObjects[randomIndex] = objectToSpawn;

            // リストから削除
            availableSpawnObjects.RemoveAt(0);

            // エリアを占有済みにする
            Destroy(spawnAreas[StageNumber].Stages[randomIndex]);
            spawnAreas[StageNumber].Stages[randomIndex] = null;
            areaOccupied[randomIndex] = true;

            CheckAllAreasFilled();

            // 生成が完了した後に、次のオブジェクトがCPUかどうかを確認する
            ListPlayer();
        }
    }

    public void ListPlayer()
    {
        if (availableSpawnObjects.Count != 0)
        {
            GameObject nextObject = availableSpawnObjects[0];

            // 次のオブジェクトがCPUの場合
            if (nextObject.tag == "NPC")
            {
                // CPUの場合は、新しい関数を呼び出してランダムなエリアに生成
                SpawnCpuObject(nextObject);
            }
            else
            {
                // 次のオブジェクトがプレイヤーの場合
                SelectNextAvailableArea();
            }
        }
    }

    private void SelectAreas(int index)
    {
        for (int i = 0; i < spawnAreas[StageNumber].Stages.Count; i++)
        {
            if (spawnAreas[StageNumber].Stages[i] != null)
            {
                //エリア選択済みかどうかでどのマテリアルを適応するか
                spawnAreas[StageNumber].Stages[i].GetComponent<Renderer>().material = areaOccupied[i] && occupiedMaterial != null ? occupiedMaterial : defaultMaterial;
            }
        }

        //選択エリアをハイライト
        if (spawnAreas[StageNumber].Stages[index] != null && !areaOccupied[index])
        {
            spawnAreas[StageNumber].Stages[index].GetComponent<Renderer>().material = highlightMaterial;
        }
    }

    private Vector3 GetSelectedSpawnPosition()
    {
        if (spawnAreas[StageNumber].Stages[currentAreaIndex] == null)
        {
            return Vector3.zero;
        }
        return spawnAreas[StageNumber].Stages[currentAreaIndex].transform.position + new Vector3(0, spawnAreas[StageNumber].Stages[currentAreaIndex].transform.localScale.y / 2, 0);
    }

    private void SelectNextAvailableArea()
    {
        int startIndex = currentAreaIndex;
        do
        {
            currentAreaIndex = (currentAreaIndex + 1) % spawnAreas[StageNumber].Stages.Count;
            if (spawnAreas[StageNumber].Stages[currentAreaIndex] != null && !areaOccupied[currentAreaIndex])
            {
                SelectAreas(currentAreaIndex);
                return;
            }
        } while (currentAreaIndex != startIndex);

        for (int i = 0; i < spawnAreas[StageNumber].Stages.Count; i++)
        {
            if (spawnAreas[StageNumber].Stages[i] != null)
            {
                spawnAreas[StageNumber].Stages[i].GetComponent<Renderer>().material = areaOccupied[i] && occupiedMaterial != null ? occupiedMaterial : defaultMaterial;
            }
        }
    }

    private void CheckAllAreasFilled()
    {
        isAllAreasFilled = true;
        gameManager.CameraControl();
        for (int i = 0; i < areaOccupied.Length; i++)
        {
            if (spawnAreas[StageNumber].Stages[i] != null && !areaOccupied[i])
            {
                isAllAreasFilled = false;
                break;
            }
        }
    }

    public void AddList()
    {
        int i = 0;

        spawnObjects = new GameObject[spawnAreas[StageNumber].Stages.Count];
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;
        }
        foreach (var player in gameManager.players)
        {
            spawnObjects[i] = player.gameObject;
            i++;
        }

        StageNumber = gameManager.StageNum;

        //スコアを低い順にソート
        availableSpawnObjects = spawnObjects.OrderBy(obj =>
        {
            var spawnObj = obj.GetComponent<PlayerBase>();
            return spawnObj != null ? spawnObj.TotalScore : float.MaxValue;
        }).ToList();


        if (spawnObjects.Length == 0) Debug.LogWarning("SpawnObjects is empty");
        foreach (var obj in availableSpawnObjects) Debug.Log($"Object: {obj.name}, Score: {obj.GetComponent<PlayerBase>()?.TotalScore}");

        // 初期状態でリストの先頭がNPCだった場合、自動で生成を開始する
        if (availableSpawnObjects.Count > 0 && availableSpawnObjects[0].tag == "NPC")
        {
            ListPlayer();
        }
        else
        {
            // 初期状態でリストの先頭がプレイヤーだった場合、最初のエリアを選択状態にする
            SelectAreas(currentAreaIndex);
        }
    }

    void OnDestroy()
    {
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Spawn.performed -= OnSpawnPerformed;
        inputActions.Player.Disable();
    }
}