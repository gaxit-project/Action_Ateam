using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultSceneManager : MonoBehaviour
{
    private GameManager gameManager;
    private CameraController cameraController;

    [Header("ResultCameraSetting")]
    [SerializeField] private Vector3 _resultCameraPosition = new Vector3();
    [SerializeField] private Vector3 _resultCameraRotation = new Vector3();

    private void Start()
    {
        cameraController = FindFirstObjectByType<CameraController>();
        gameManager = FindFirstObjectByType<GameManager>();

        gameManager.ResultSetting();
        SetPlayerHeightsByRank();
        SetResultCameraMode();

    }

    /// <summary>
    /// リザルト時のカメラ位置・向き設定
    /// </summary>
    public void SetResultCameraMode()
    {
        if(cameraController == null)
        {
            Debug.LogError("CameraControllerNULL");
            return;
        }
        cameraController.StopCameraMove();
        cameraController.ResultCamera(_resultCameraPosition, _resultCameraRotation);


        Debug.Log("カメラをリザルトモードに設定");
    }
    public void SetPlayerHeightsByRank()
    {
        foreach(var player in gameManager.players)
        {
            gameManager.RankSort();
            
            int rank = player.Rank;

            float baseHeight = 10f;
            float step = 2f;

            float y = baseHeight - (rank - 1) * step;
            if (y < 1f) y = 1f;

            Vector3 pos = player.transform.position;

            player.transform.position = new Vector3(pos.x, y, pos.z);

            var rb = player.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }
        }
    }
}
