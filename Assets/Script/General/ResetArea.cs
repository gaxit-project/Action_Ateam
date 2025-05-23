using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetArea : MonoBehaviour
{
    public bool isPlayerOut = false;

    public void ResetGame()
    {
        SceneManager.LoadScene(1);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("エリア外に到達しました");
            isPlayerOut = true;
            GameManager.Instance.currentFrameResult();
        }
    }
}
