//未完成
using UnityEngine;
using System.Collections.Generic;

public class SpeedDownOnTouchOnce : MonoBehaviour
{
    // すでにスケール済みのオブジェクトを記録する
    private HashSet<Transform> SpeedDownBafuAria = new HashSet<Transform>();

    private void OnTriggerEnter(Collider other)
    {

        Transform SpeedDownPlayer = other.transform;

        // まだ触れてないオブジェクトのみ処理
        if (!SpeedDownBafuAria.Contains(SpeedDownPlayer))
        {
            //↓ここから修正したい
            SpeedDownPlayer.localScale *= 1.1f;
            AudioManager.Instance.PlaySound(2);
            SpeedDownBafuAria.Add(SpeedDownPlayer);
        }
    }
}
