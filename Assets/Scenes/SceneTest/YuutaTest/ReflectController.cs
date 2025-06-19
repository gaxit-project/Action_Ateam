using UnityEngine;

public class ReflectController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            Player player = collision.gameObject.GetComponent<Player>();
            if(rb != null && player != null && player.PlayerStateProperty == Player.PlayerState.Run)
            {
                // 現在の進行方向（速度）
                Vector3 incomingVelocity = rb.linearVelocity;

                // 衝突面の法線
                Vector3 normal = collision.contacts[0].normal;

                // 反射ベクトルの計算
                Vector3 reflectVelocity = Vector3.Reflect(incomingVelocity, normal).normalized;

                // 法線方向に少し押し返しを加える
                reflectVelocity += normal * 0.2f;

                reflectVelocity.Normalize();

                player.Reflect(reflectVelocity);
            }
        }
    }
}
