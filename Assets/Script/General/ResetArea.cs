using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetArea : MonoBehaviour
{
    bool isPlayerOut = false;

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            ResetGame();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Playerがエリア外に到達");
            isPlayerOut = true;
        }
    }

    private void ResetGame()
    {
        if (isPlayerOut)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.Log("Playerはエリア内です");
        }
    }
}
