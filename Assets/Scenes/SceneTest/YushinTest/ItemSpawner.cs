using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject buffItemPrefab;
    [SerializeField] private BuffItem[] buffItems;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxItems = 5;
    [SerializeField] private GameObject spawnBox;
    [SerializeField] private Transform spawnParent;

    private int currentItemCount = 0;
    private BoxCollider boxCollider;

    private void Start()
    {
        if (buffItemPrefab == null || buffItems.Length == 0) return;
        if (spawnBox == null) return;

        boxCollider = spawnBox.GetComponent<BoxCollider>();
        if (boxCollider == null) return;

        StartCoroutine(SpawnItems());
    }

    private IEnumerator SpawnItems()
    {
        while (true)
        {
            if(currentItemCount < maxItems)
            {
                SpawnItem();
                currentItemCount++;
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnItem()
    {
        BuffItem buff = buffItems[Random.Range(0, buffItems.Length)];
        Vector3 spawnPos = GetRandomPointInBoxCollider();
        GameObject item = Instantiate(buffItemPrefab, spawnPos, Quaternion.identity, spawnParent);
        item.tag = "BuffItem";
        BuffItemInstance component = item.GetComponent<BuffItemInstance>();
        if(component != null)
        {
            component.SetBuffItem(buff);
        }
        item.AddComponent<ItemTracker>().Initialize(this);
    }

    private Vector3 GetRandomPointInBoxCollider()
    {
        Bounds bounds = boxCollider .bounds;
        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;
        Vector3 localPoint = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z)
        );
        return center + boxCollider.transform.TransformVector(localPoint);
    }

    public void OnItemDestroyed()
    {
        currentItemCount--;
    }

    private void OnDrawGizmos()
    {
        if(spawnBox != null && spawnBox.GetComponent<BoxCollider>() != null)
        {
            Gizmos.color = Color.green;
            BoxCollider collider = spawnBox.GetComponent<BoxCollider>();
            Gizmos.matrix = Matrix4x4.TRS(spawnBox.transform.position, spawnBox.transform.rotation, spawnBox.transform.lossyScale);
            Gizmos.DrawWireCube(collider.center, collider.size);
        }
    }
}

public class ItemTracker : MonoBehaviour
{
    private ItemSpawner spawner;

    public void Initialize(ItemSpawner spawner)
    {
        this.spawner = spawner;
    }

    private void OnDestroy()
    {
        if(spawner != null)
        {
            spawner.OnItemDestroyed();
        }
    }
}