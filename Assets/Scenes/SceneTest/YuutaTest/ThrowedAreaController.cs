using UnityEngine;

public class ThrowedAreaController : MonoBehaviour
{


    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerBase player = other.GetComponent<PlayerBase>();
            if (player) player.Throw();
            else Debug.LogError("PlayerBaseがアタッチされていません!");
        }
    }

}
