using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetArea : MonoBehaviour
{
    public bool isPlayerOut = false;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private string SceneName;
    private int BuildIndex;
    private void Update()
    {
        BuildIndex = SceneManager.GetActiveScene().buildIndex;
        if(BuildIndex == 1 || BuildIndex == 4)
        {
            scoreManager = FindFirstObjectByType<ScoreManager>();
        }
    }
    public void ResetGame()
    {
        scoreManager.FrameSaveSystem();
        SceneChangeManager.Instance.ResetScene(SceneName);
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("エリア外に到達しました");
            isPlayerOut = true;
            GameManager.Instance.CurrentFrameResult();
        }
    }
}
