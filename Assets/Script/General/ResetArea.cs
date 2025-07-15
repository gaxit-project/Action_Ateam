using NPC.StateAI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetArea : MonoBehaviour
{
    private int BuildIndex;
    private DisplayScore DS;
    private void Update()
    {
        BuildIndex = SceneManager.GetActiveScene().buildIndex;
        if (BuildIndex == 1 || BuildIndex == 4)
        {
            DS = GameObject.FindFirstObjectByType<DisplayScore>();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log($"{collision.GetComponent<Player>().GetPlayerID()}がエリア外に到達しました");
            DS.PlayerFall();
        }
        if (collision.gameObject.tag == "NPC")
        {
            Debug.Log($"{collision.GetComponent<EnemyAI>().GetPlayerID()}がエリア外に到達しました");
            DS.NPCFall();
        }
    }
}
