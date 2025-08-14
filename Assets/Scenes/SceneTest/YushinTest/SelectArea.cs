using UnityEngine;
using UnityEngine.InputSystem;

public class SelectArea : MonoBehaviour
{
    public GameObject[] spawnObjects; //生成するプレイヤーの配列
    private int currentObjectIndex = 0; //次に生成するオブジェクトのインデックス

    public GameObject[] spawnAreas;
    public Material highlightMaterial;
    private Material[] defaultMaterials;
    private int currentAreaIndex = 0;
    private PlayerInputActions inputActions;
    private float lastInputTime = 0f;
    private float inputCooldown = 0.3f;
    private float lastSpawnTime = 0f;
    private float spawnCooldown = 0.5f;
    private bool[] areaOccupied;
    public bool isAllAreasFilled = false;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Spawn.performed += OnSpawnPerformed;

        defaultMaterials = new Material[spawnAreas.Length];
        areaOccupied = new bool[spawnAreas.Length];
        for(int i = 0; i < spawnAreas.Length; i++)
        {
            defaultMaterials[i] = spawnAreas[i].GetComponent<Renderer>().material;
            areaOccupied[i] = false;
        }

        SelectAreas(currentAreaIndex);

        if (spawnObjects.Length == 0) Debug.LogWarning("SpawnObjects is empty");
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        //入力重複対策
        if (Time.time - lastInputTime < inputCooldown) return;

        //スティック入力
        float horizontalInput = inputActions.Player.Move.ReadValue<Vector2>().x;

        if (Mathf.Abs(horizontalInput) > 0.5f)
        {
            int previousIndex = currentAreaIndex;
            //左入力
            if (horizontalInput < 0)
            {
                do
                {
                    currentAreaIndex = (currentAreaIndex - 1 + spawnAreas.Length) % spawnAreas.Length;
                } while (areaOccupied[currentAreaIndex] && currentAreaIndex != previousIndex);
            }
            //右入力
            else if (horizontalInput > 0)
            {
                do
                {
                    currentAreaIndex = (currentAreaIndex + 1 + spawnAreas.Length) % spawnAreas.Length;
                } while (areaOccupied[currentAreaIndex] && currentAreaIndex != previousIndex);
            }

            if (!areaOccupied[currentAreaIndex])
            {
                SelectAreas(currentAreaIndex); //選択更新
            }
            lastInputTime = Time.time; //入力時刻更新
        }
    }

    private void OnSpawnPerformed(InputAction.CallbackContext context)
    {
        if (Time.time - lastSpawnTime < spawnCooldown || spawnObjects.Length == 0 || areaOccupied[currentAreaIndex]) return;

        Vector3 spawnPosition = GetSelectedSpawnPosition();
        GameObject spawnedObject = Instantiate(spawnObjects[currentObjectIndex], spawnPosition, Quaternion.identity);

        areaOccupied[currentAreaIndex] = true;

        currentObjectIndex = (currentObjectIndex + 1) % spawnObjects.Length;

        CheckAllAreasFilled();

        SelectNextAvailableArea();

        lastSpawnTime = Time.time;
    }

    private void SelectAreas(int index)
    {
        for (int i = 0; i < spawnAreas.Length; i++)
        {
            spawnAreas[i].GetComponent<Renderer>().material = defaultMaterials[i];
        }

        //選択エリアをハイライト
        if (!areaOccupied[index])
        {
            spawnAreas[index].GetComponent<Renderer>().material = highlightMaterial;
            Debug.Log("Selected Area: " + spawnAreas[index].name + " at " + spawnAreas[index].transform.position);

        }
    }

    private Vector3 GetSelectedSpawnPosition()
    {
        return spawnAreas[currentAreaIndex].transform.position + new Vector3(0, spawnAreas[currentAreaIndex].transform.localScale.y / 2, 0);
    }

    private void SelectNextAvailableArea()
    {
        int startIndex = currentAreaIndex;
        do
        {
            currentAreaIndex = (currentAreaIndex + 1) % spawnAreas.Length;
            if (!areaOccupied[startIndex])
            {
                SelectAreas(currentAreaIndex);
                return;
            }
        }while(currentAreaIndex !=  startIndex);

        for(int i = 0; i < spawnAreas.Length; i++)
        {
            spawnAreas[i].GetComponent<Renderer>().material = defaultMaterials[i];
        }
    }

    private void CheckAllAreasFilled()
    {
        isAllAreasFilled = true;
        for(int i = 0; i < areaOccupied.Length; i++)
        {
            if (!areaOccupied[i])
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
