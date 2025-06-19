using UnityEngine;

public class DeathZoneController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if(player != null && player.PlayerStateProperty == Player.PlayerState.Run)
                player.PlayerStateProperty = Player.PlayerState.Dead;
        }
    }
}

