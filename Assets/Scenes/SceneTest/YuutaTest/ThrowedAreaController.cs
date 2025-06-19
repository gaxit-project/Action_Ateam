using UnityEngine;

public class ThrowedAreaController : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player) player.Throw();
        }
    }

}
