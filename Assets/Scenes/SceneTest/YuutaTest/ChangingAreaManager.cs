using UnityEngine;

public class ChangingAreaManager : MonoBehaviour
{
    [SerializeField] private bool resetControllMode = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(!resetControllMode) other.gameObject.GetComponent<Player>().isControllEasily = true;
            else other.gameObject.GetComponent<Player>().isControllEasily = false;
        }
    }
}
