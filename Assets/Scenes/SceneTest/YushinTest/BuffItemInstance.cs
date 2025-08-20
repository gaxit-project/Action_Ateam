using UnityEngine;

public class BuffItemInstance : MonoBehaviour
{
    [SerializeField] private BuffItem buffItem;
    public BuffItem BuffItem => buffItem;

    public void SetBuffItem(BuffItem buff)
    {
        buffItem = buff;
    }

    void Start()
    {
        if (!gameObject.CompareTag("BuffItem"))
        {
            gameObject.tag = "BuffItem";
        }
        Collider collider = GetComponent<Collider>();
        if(collider != null && !collider.isTrigger)
        {
            collider.isTrigger = true;
        }
    }
}
