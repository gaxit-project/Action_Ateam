using UnityEngine;

public class BoundSE : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("NPC"))
        {
            AudioManager.Instance.PlaySound(2);
        }
    }
}
