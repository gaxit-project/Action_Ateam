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
        if(scoreManager.isFinish == true)
        {
            SceneChangeManager.Instance.SceneChange("Result");
        }
        else
        {
            SceneChangeManager.Instance.ResetScene(SceneName);
        }

        
    }

    public void TimeOver()
    {
        Debug.Log("時間切れです");
        cameraController.StopCameraMove();
        DisplayScore();
    }

    private void DisplayScore()
    {
        isPlayerOut = true;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var p in players) p.gameObject.SetActive(false);
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
        foreach (var n in npcs) n.gameObject.SetActive(false);
        GameObject[] pins = GameObject.FindGameObjectsWithTag("Pin");
        foreach (var pin in pins) pin.gameObject.SetActive(false);
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
