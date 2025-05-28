using UnityEngine;
using System.Collections.Generic;

public class ScaleOnTouchOnce : MonoBehaviour
{
    // すでにスケール済みのオブジェクトを記録する
    private HashSet<Transform> scaledObjects = new HashSet<Transform>();

    private void OnTriggerEnter(Collider other)
    {
        Transform BigPlayer = other.transform;

        // まだ触れていないオブジェクトのみ処理
        if (!scaledObjects.Contains(BigPlayer))
        {
            BigPlayer.localScale *= 1.1f;
            AudioManager.Instance.PlaySound(2);
            scaledObjects.Add(BigPlayer);
        }
    }
}
