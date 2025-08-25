using UnityEngine;

public class IconController : MonoBehaviour
{
    void Update()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 90f, 0);
    }
}
