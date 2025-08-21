using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayScore : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private string SceneName = "Main";
    private int BuildIndex;
    private FrameMoveCameraScript frameMoveCameraScript;
    private FrameStarterScript_2 frameStarterScript_2;
    private GameManager gameManager;
    private CameraController cameraController;

    private int fallCount = 0;
    private int falledPlayerCount = 0;
    private int falledNPCCount = 0;

    private bool isFalledAll = false;

    private void Update()
    {
        BuildIndex = SceneManager.GetActiveScene().buildIndex;
        if (BuildIndex == 1 || BuildIndex == 4)
        {
            frameStarterScript_2 = FindFirstObjectByType<FrameStarterScript_2>();
            scoreManager = FindFirstObjectByType<ScoreManager>();
            frameMoveCameraScript = FindFirstObjectByType<FrameMoveCameraScript>();
            gameManager = FindFirstObjectByType<GameManager>();
            cameraController = FindFirstObjectByType<CameraController>();
        }

        if(!isFalledAll && fallCount == gameManager.NumHumanPlayers + gameManager.NumBots)
        {
            isFalledAll = true;
            Debug.Log("参加者全員が落下しました");
            Display();
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
        if (scoreManager.isFinish == true)
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
        Display();
    }

    public void Display()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var p in players) p.gameObject.SetActive(false);
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
        foreach (var n in npcs) n.gameObject.SetActive(false);
        GameObject[] pins = GameObject.FindGameObjectsWithTag("Pin");
        foreach (var pin in pins) pin.gameObject.SetActive(false);
        GameObject[] pinCircle = GameObject.FindGameObjectsWithTag("PinCircle");
        foreach (var p in pinCircle) p.gameObject.SetActive(false);

        GameObject camObj = GameObject.FindGameObjectWithTag("MainCamera");
            Camera mainCam = camObj.GetComponent<Camera>();
            mainCam.orthographic = true;
            mainCam.orthographicSize = 40f; // 表示範囲の調整
            mainCam.transform.rotation = Quaternion.Euler(30, 45, 0);
            gameManager.StopTimer();
        AudioManager.Instance.PlayBGM(2);
        frameMoveCameraScript.WarpCameraToMenObject();
        frameStarterScript_2.FrameObjectUPFalse();
        GameManager.Instance.CurrentFrameResult();
    }

    public void PlayerFall()
    {
        falledPlayerCount++;
        if (falledPlayerCount > gameManager.NumHumanPlayers) Debug.LogError("参加player数以上のplayerが落下しました！");
        fallCount++;
        Debug.Log(fallCount);
    }

    public void NPCFall()
    {
        falledNPCCount++;
        if (falledNPCCount > gameManager.NumBots) Debug.LogError("参加NPC数以上のNPCが落下しました！");
        fallCount++;
        Debug.Log(fallCount);
    }
}
