using System.Threading;
using UnityEngine;

public class PinBase : MonoBehaviour
{
    [SerializeField] private float knockDownAngleThreshold = 45f;
    [SerializeField] private GameObject outAreaObj;

    public Vector3 center = new Vector3(0f, -0.6f, 0f);
    private Rigidbody rb;

    private Quaternion initialRotation;
    private bool isFallDown = false;



    private void Start()
    {
        isFallDown = false;
        initialRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.centerOfMass = center;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Pin" || other.gameObject.tag == "Player")
        {
            AudioManager.Instance.PlaySound(1);
        }
    }

    private void OnTriggerEnter(Collider outAreaObj)
    {
        isFallDown = true;
    }

    public bool IsKnockedDownPin()
    {
        if (isFallDown)
        {
            return true;
        }

        float angle = Quaternion.Angle(initialRotation, transform.rotation);
        if(angle > knockDownAngleThreshold)
        {
            isFallDown = true;
            return true;
        }
        return false;
    }
}
