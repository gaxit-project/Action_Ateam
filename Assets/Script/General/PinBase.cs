using System.Threading;
using UnityEngine;

public class PinBase : MonoBehaviour
{
    [SerializeField] private float knockDownAngleThreshold = 45f;

    private Quaternion initialRotation;

    private void Start()
    {
        initialRotation = transform.rotation;
    }

    public bool IsKnockedDownPin()
    {
        float angle = Quaternion.Angle(initialRotation, transform.rotation);
        return angle > knockDownAngleThreshold;
    }
}
