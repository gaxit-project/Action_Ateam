using System.Collections;
using UnityEngine;

public class AttackAreaController : MonoBehaviour
{
    private GameObject player;
    Rigidbody rb;

    void Start()
    {
        StartCoroutine(DelayedInit());
    }

    private IEnumerator DelayedInit()
    {
        // 1フレーム待つ（他の Start や Awake が終わるのを待つ）
        yield return null;

        if (transform.parent != null) player = transform.parent.gameObject;
        else Debug.LogError("親オブジェクトが存在しません！");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != player && other.CompareTag("Player"))
        {
            PlayerBase playerBase = other.GetComponent<PlayerBase>();
            if (playerBase == null)
            {
                rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity += transform.right * 10f;
                }
            }
            else playerBase.Attacked(transform.right * 10f);
        }
    }
}
