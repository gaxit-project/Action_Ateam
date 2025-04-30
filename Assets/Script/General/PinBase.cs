using System.Threading;
using UnityEngine;

public class PinBase : MonoBehaviour
{
    [SerializeField] private float knockDownAngleThreshold = 45f;

    public Vector3 center = new Vector3(0f, -0.6f, 0f);
    private Rigidbody rb;

    private Quaternion initialRotation;

    private void Start()
    {
        initialRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.centerOfMass = center;
        }
    }

    public bool IsKnockedDownPin()
    {
        float angle = Quaternion.Angle(initialRotation, transform.rotation);
        return angle > knockDownAngleThreshold;
    }
}
