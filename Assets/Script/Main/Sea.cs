using UnityEngine;

public class Sea : MonoBehaviour
{
    public ParticleSystem effectPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("NPC") || collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Pin"))
        {
            if (effectPrefab != null)
            {
                ParticleSystem effect = Instantiate(effectPrefab,
                    collision.contacts[0].point,
                    Quaternion.identity);

                Destroy(effect.gameObject, effect.main.duration);
            }

            Destroy(collision.gameObject);
        }
    }
}
