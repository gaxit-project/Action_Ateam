using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject buffItemPrefab;
    [SerializeField] private BuffItem[] buffItems;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private GameObject[] spawnBoxes;
    [SerializeField] private Transform spawnParent;

    private int maxItemsPerBox = 3;
    private int[] currentItemCount;
    private BoxCollider[] boxColliders;

    private void Start()
    {
        if (buffItemPrefab == null || buffItems.Length == 0) return;
        if (spawnBoxes == null) return;

        boxColliders = new BoxCollider[spawnBoxes.Length];
        currentItemCount = new int[spawnBoxes.Length];
        for(int i = 0; i < spawnBoxes.Length; i++)
        {
            if (spawnBoxes[i] == null) return;
            boxColliders[i] = spawnBoxes[i].GetComponent<BoxCollider>();
            currentItemCount[i] = 0;
        }

        StartCoroutine(SpawnItems());
    }

    private IEnumerator SpawnItems()
    {
        while (true)
        {
            for(int i = 0; i < spawnBoxes.Length; i++)
            {
                if (currentItemCount[i] < maxItemsPerBox)
                {
                    SpawnItem(i);
                    currentItemCount[i]++;
                }
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    private void SpawnItem(int boxIndex)
    {
        BuffItem buff = buffItems[Random.Range(0, buffItems.Length)];
        Vector3 spawnPos = GetRandomPointInBoxCollider(boxIndex);
        GameObject item = Instantiate(buffItemPrefab, spawnPos, Quaternion.identity, spawnParent);
        item.tag = "BuffItem";
        BuffItemInstance component = item.GetComponent<BuffItemInstance>();
        if(component != null)
        {
            component.SetBuffItem(buff);
        }
        item.AddComponent<ItemTracker>().Initialize(this, boxIndex);
    }

    private Vector3 GetRandomPointInBoxCollider(int boxIndex)
    {
        Bounds bounds = boxColliders[boxIndex].bounds;
        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;
        Vector3 localPoint = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z)
        );
        return center + boxColliders[boxIndex].transform.TransformVector(localPoint);
    }

    public void OnItemDestroyed(int boxIndex)
    {
        if(boxIndex >= 0 && boxIndex < boxColliders.Length)
        {
            currentItemCount[boxIndex]--;
        }
    }

    private void OnDrawGizmos()
    {
        if(spawnBoxes != null)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < spawnBoxes.Length; i++)
            {
                if(spawnBoxes[i] != null && spawnBoxes[i].GetComponent<BoxCollider>() != null)
                {
                    BoxCollider collider = spawnBoxes[i].GetComponent<BoxCollider>();
                    Gizmos.matrix = Matrix4x4.TRS(spawnBoxes[i].transform.position, spawnBoxes[i].transform.rotation, spawnBoxes[i].transform.lossyScale);
                    Gizmos.DrawWireCube(collider.center, collider.size);
                }
            }
        }
    }
}

public class ItemTracker : MonoBehaviour
{
    private ItemSpawner spawner;
    private int boxIndex;

    public void Initialize(ItemSpawner spawner, int boxIndex)
    {
        this.spawner = spawner;
        this.boxIndex = boxIndex;
    }

    private void OnDestroy()
    {
        if(spawner != null)
        {
            spawner.OnItemDestroyed(boxIndex);
        }
    }
}