using System.Runtime.CompilerServices;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {

        }
    }
}
