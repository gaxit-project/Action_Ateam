using UnityEngine;

public class Sea : MonoBehaviour
{
    public ParticleSystem effectPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC") || other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Pin"))
        {
            MeshRenderer mesh = other.gameObject.GetComponent<MeshRenderer>();
            if (effectPrefab != null && mesh.enabled)
            {
                ParticleSystem effect = Instantiate(effectPrefab,
                    other.transform.position,
                    Quaternion.identity);

                Destroy(effect.gameObject, effect.main.duration);

                //Œ©‚½–Ú‚¾‚¯‚ğÁ‚·
                mesh.enabled = false;
                Transform children = other.gameObject.GetComponentInChildren<Transform>();
                foreach (Transform child in children)
                {
                    child.gameObject.SetActive(false);
                }
                //player‚ªŠC‚É“ü‚Á‚½‚çƒJƒƒ‰‚ğ~‚ß‚é
                if (other.gameObject.CompareTag("Player"))
                {
                    CameraController cam = GameObject.FindFirstObjectByType<CameraController>();
                    cam.StopCameraMove();
                    Debug.Log("ƒJƒƒ‰’â~");
                }
            }

            //Destroy(other.gameObject);
        }
    }
}
