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
    private FrameStarterScript_2 frameStarterScript_2;
    private GameManager gameManager;
    private CameraController cameraController;
    private void Update()
    {
        BuildIndex = SceneManager.GetActiveScene().buildIndex;
        if(BuildIndex == 1 || BuildIndex == 4)
        {
            frameStarterScript_2 = FindFirstObjectByType<FrameStarterScript_2>();
            scoreManager = FindFirstObjectByType<ScoreManager>();
            frameMoveCameraScript = FindFirstObjectByType<FrameMoveCameraScript>();
            gameManager = FindFirstObjectByType<GameManager>();
            cameraController = FindFirstObjectByType<CameraController>();
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

    public void TimeOver()
    {
        Debug.Log("時間切れです");
        cameraController.StopCameraMove();
        GameObject[] areas = GameObject.FindGameObjectsWithTag("ThrowArea");
        foreach (var area in areas) area.gameObject.SetActive(false);
        DisplayScore();
    }

    private void DisplayScore()
    {
        isPlayerOut = true;
        gameManager.StopTimer();
        AudioManager.Instance.PlayBGM(2);
        frameMoveCameraScript.WarpCameraToMenObject();
        frameStarterScript_2.FrameObjectUPFalse();
        GameManager.Instance.CurrentFrameResult();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("エリア外に到達しました");
            DisplayScore();
        }
    }
}
