using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetArea : MonoBehaviour
{
    public bool isPlayerOut = false;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private string SceneName;
    private int BuildIndex;
    private FrameMoveCameraScript frameMoveCameraScript;
    private FrameStarterScript frameStarterScript;
    private void Update()
    {
        BuildIndex = SceneManager.GetActiveScene().buildIndex;
        if(BuildIndex == 1 || BuildIndex == 4)
        {
            frameStarterScript = FindFirstObjectByType<FrameStarterScript>();
            scoreManager = FindFirstObjectByType<ScoreManager>();
            frameMoveCameraScript = FindFirstObjectByType<FrameMoveCameraScript>();
        }
    }

    /// <summary>
    /// フレームリセット
    /// </summary>
    /// <returns>3秒待ってからリセット</returns>
    public IEnumerator ResetGame()
    {
        scoreManager.FrameSaveSystem();

        yield return new WaitForSeconds(3f);
        foreach (var player in GameManager.Instance.players)
        {
            if (player != null)
                Destroy(player.gameObject);
        }
        GameManager.Instance.players.Clear();
        SceneChangeManager.Instance.ResetScene(SceneName);
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("エリア外に到達しました");
            isPlayerOut = true;
            AudioManager.Instance.PlayBGM(2);
            frameMoveCameraScript.WarpCameraToMenObject();
            //frameStarterScript.FrameObjectLeftFalse();
            GameManager.Instance.CurrentFrameResult();
        }
    }
}
