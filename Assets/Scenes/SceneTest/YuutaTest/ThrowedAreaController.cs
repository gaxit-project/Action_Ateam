using UnityEngine;

public class ThrowedAreaController : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerBase player = other.GetComponent<PlayerBase>();
            if (player) player.Throw();
        }
    }

}
