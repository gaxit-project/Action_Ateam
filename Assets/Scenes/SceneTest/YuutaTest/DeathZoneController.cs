using UnityEngine;

public class DeathZoneController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerBase player = other.GetComponent<PlayerBase>();
            if(player != null && player.PlayerStateProperty == PlayerState.Run)
                player.PlayerStateProperty = PlayerState.Dead;
        }
    }
}

