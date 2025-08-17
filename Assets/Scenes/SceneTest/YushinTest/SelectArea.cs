using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

public class SelectArea : MonoBehaviour
{
    public GameObject[] spawnObjects; //生成するプレイヤーの配列
    private int currentObjectIndex = 0; //次に生成するオブジェクトのインデックス

    public GameObject[] spawnAreas;
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
    private List<GameObject> spawnedObjects;
    private List<GameObject> availableSpawnObjects;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Spawn.performed += OnSpawnPerformed;

        //エリア初期化
        areaOccupied = new bool[spawnAreas.Length];
        spawnedObjects = new List<GameObject>(new GameObject[spawnAreas.Length]);
        for(int i = 0; i < spawnAreas.Length; i++)
        {
            if (spawnAreas[i] != null)
            {
                areaOccupied[i] = false;
                spawnedObjects.Add(null);
                spawnAreas[i].GetComponent<Renderer>().material = defaultMaterial;
            }
        }

        //スコアを低い順にソート
        availableSpawnObjects = spawnObjects.OrderBy(obj =>
        {
            var spawnObj = obj.GetComponent<SpawnObject>();
            return spawnObj != null ? spawnObj.score : float.MaxValue;
        }).ToList();

        SelectAreas(currentAreaIndex);

        if (spawnObjects.Length == 0) Debug.LogWarning("SpawnObjects is empty");
        foreach (var obj in availableSpawnObjects) Debug.Log($"Object: {obj.name}, Score: {obj.GetComponent<SpawnObject>()?.score}");
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
                    currentAreaIndex = (currentAreaIndex - 1 + spawnAreas.Length) % spawnAreas.Length;
                } while ((spawnAreas[currentAreaIndex] == null || areaOccupied[currentAreaIndex]) && currentAreaIndex != previousIndex);
            }
            //右入力
            else if (horizontalInput > 0)
            {
                do
                {
                    currentAreaIndex = (currentAreaIndex + 1) % spawnAreas.Length;
                } while ((spawnAreas[currentAreaIndex] == null || areaOccupied[currentAreaIndex]) && currentAreaIndex != previousIndex);
            }

            if (spawnAreas[currentAreaIndex] != null && !areaOccupied[currentAreaIndex])
            {
                SelectAreas(currentAreaIndex);
            }
            lastInputTime = Time.time; //入力時刻更新
        }
    }

    private void OnSpawnPerformed(InputAction.CallbackContext context)
    {
        if (Time.time - lastSpawnTime < spawnCooldown || spawnObjects.Length == 0 || areaOccupied[currentAreaIndex]) return;

        //オブジェクト生成
        GameObject nextObject = availableSpawnObjects[0];
        Vector3 spawnPosition = GetSelectedSpawnPosition();
        GameObject spawnedObject = Instantiate(nextObject, spawnPosition, Quaternion.identity);
        spawnedObjects[currentAreaIndex] = spawnedObject;

        //オブジェクトをリストから削除
        availableSpawnObjects.RemoveAt(0);

        //エリア削除
        Destroy(spawnAreas[currentAreaIndex]);
        spawnAreas[currentAreaIndex] = null;
        areaOccupied[currentAreaIndex] = true;

        CheckAllAreasFilled();

        SelectNextAvailableArea();

        lastSpawnTime = Time.time;
    }

    private void SelectAreas(int index)
    {
        for (int i = 0; i < spawnAreas.Length; i++)
        {
            if (spawnAreas[i] != null)
            {
                //エリア選択済みかどうかでどのマテリアルを適応するか
                spawnAreas[i].GetComponent<Renderer>().material = areaOccupied[i] && occupiedMaterial != null ? occupiedMaterial : defaultMaterial;
            }
        }

        //選択エリアをハイライト
        if (spawnAreas[index] != null && !areaOccupied[index])
        {
            spawnAreas[index].GetComponent<Renderer>().material = highlightMaterial;
        }
    }

    private Vector3 GetSelectedSpawnPosition()
    {
        if(spawnAreas[currentAreaIndex] == null)
        {
            return Vector3.zero;
        }
        return spawnAreas[currentAreaIndex].transform.position + new Vector3(0, spawnAreas[currentAreaIndex].transform.localScale.y / 2, 0);
    }

    private void SelectNextAvailableArea()
    {
        int startIndex = currentAreaIndex;
        do
        {
            currentAreaIndex = (currentAreaIndex + 1) % spawnAreas.Length;
            if (spawnAreas[currentAreaIndex] != null && !areaOccupied[currentAreaIndex])
            {
                SelectAreas(currentAreaIndex);
                return;
            }
        }while(currentAreaIndex !=  startIndex);

        for (int i = 0; i < spawnAreas.Length; i++)
        {
            if (spawnAreas[i] != null)
            {
                spawnAreas[i].GetComponent<Renderer>().material = areaOccupied[i] && occupiedMaterial != null ? occupiedMaterial : defaultMaterial;
            }
        }
    }

    private void CheckAllAreasFilled()
    {
        isAllAreasFilled = true;
        for(int i = 0; i < areaOccupied.Length; i++)
        {
            if (spawnAreas[i] != null && !areaOccupied[i])
            {
                isAllAreasFilled = false;
                break;
            }
        }
    }

    void OnDestroy()
    {
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Spawn.performed -= OnSpawnPerformed;
        inputActions.Player.Disable();
    }
}
