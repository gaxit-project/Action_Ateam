using UnityEngine;
using System.Collections;

public class BoundSE : MonoBehaviour
{
    private const string EFFECT_PATH = "Effect/Collision";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("NPC"))
        {
            AudioManager.Instance.PlaySound(2);
        }
        if (collision.gameObject.CompareTag("Wall")){
            AudioManager.Instance.PlaySound(3);
        }
    }
}
