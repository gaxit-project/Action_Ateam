using UnityEngine;

public class ScaleOnTouch : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 触れたオブジェクトの現在のサイズを取得し、1.1倍にする
        other.transform.localScale *= 1.1f;
    }
}