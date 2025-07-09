using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using System.Collections; // Coroutineのために追加

public class ResultSceneManager : MonoBehaviour
{
    private GameManager gameManager;
    private CameraController cameraController;

    [Header("ResultCameraSetting")]
    [SerializeField] private Vector3 _resultCameraPosition = new Vector3();
    [SerializeField] private Vector3 _resultCameraRotation = new Vector3();

    [Header("Rank")]
    [SerializeField] private Vector3 _posRank1 = new Vector3();
    [SerializeField]private Vector3 _rotRank1 = new Vector3();
    [SerializeField] private Vector3 _posRank2 = new Vector3();
    [SerializeField] private Vector3 _rotRank2 = new Vector3();
    [SerializeField] private Vector3 _posRank3 = new Vector3();
    [SerializeField] private Vector3 _rotRank3 = new Vector3();
    [SerializeField] private Vector3 _posRank4 = new Vector3();
    [SerializeField] private Vector3 _rotRank4 = new Vector3();

    private Vector3 newPosition;
    private Quaternion newRotation;
    private void Start()
    {
        cameraController = FindFirstObjectByType<CameraController>();
        gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManagerが見つかりません。シングルトンが正しく設定されているか確認してください。");
            return;
        }

        // コルーチンを開始して、GameManagerの準備を待つ
        StartCoroutine(SetupResultScene());
    }

    private IEnumerator SetupResultScene()
    {
        Debug.Log("IENUMERATORSTART");
        gameManager.ResultSetting(); // GameManagerがRankSort()などを内部で呼ぶことを期待

        // GameManager.players リストが準備されるまで待機する
        // ループ回数や条件は、GameManagerの処理速度に合わせて調整してください。
        // あるいは、GameManagerから「準備完了」のイベントを受け取るのが理想的です。
        int maxAttempts = 10;
        int currentAttempt = 0;
        while ((gameManager.players == null || gameManager.players.Count == 0) && currentAttempt < maxAttempts)
        {
            Debug.Log("GameManager.players の準備を待っています...");
            yield return null; // 1フレーム待つ
            currentAttempt++;
        }

        if (gameManager.players == null || gameManager.players.Count == 0)
        {
            Debug.LogError("GameManagerのplayersリストが時間内に準備できませんでした。");
            yield break; // コルーチンを終了
        }

        // GameManagerが生成したプレイヤーオブジェクト（ボール）のY座標をランクに応じて調整
        SetPlayerHeightsByRank();

        // カメラをリザルトモードに設定
        SetResultCameraMode();
    }

    /// <summary>
    /// リザルト時のカメラ位置・向き設定
    /// </summary>
    public void SetResultCameraMode()
    {
        if (cameraController == null)
        {
            Debug.LogError("CameraControllerNULL");
            return;
        }
        Debug.Log("CameraModeCahnge");
        cameraController.StopCameraMove();
        cameraController.ResultCameraMode(_resultCameraPosition, _resultCameraRotation);

        Debug.Log("カメラをリザルトモードに設定");
    }

    /// <summary>
    /// ランクに応じてプレイヤー（ボール）のXY座標を設定
    /// </summary>
    public void SetPlayerHeightsByRank()
    {
        if (gameManager.players == null || gameManager.players.Count == 0)
        {
            Debug.LogWarning("GameManagerのplayersリストが空です。リザルトボールが生成されていません。");
            return;
        }

        var sortedPlayers = gameManager.players.OrderBy(p => p.Rank).ToList();
        foreach ( var player in sortedPlayers)
        {
            int rank = player.Rank;
            

            switch (rank)
            {
                case 1:
                    newPosition = _posRank1;
                    newRotation = Quaternion.Euler(_rotRank1);
                    break;
                case 2:
                    newPosition = _posRank2;
                    newRotation = Quaternion.Euler(_rotRank2);
                    break;
                case 3:
                    newPosition = _posRank3;
                    newRotation = Quaternion.Euler(_rotRank3);
                    break;
                case 4:
                    newPosition = _posRank4;
                    newRotation = Quaternion.Euler(_rotRank4);
                    break;
                default:
                    Debug.LogError("順位が定まっていません");
                    break;
            }
            
            // プレイヤーのTransformを更新
            player.transform.position = newPosition;
            player.transform.rotation = newRotation;

            // Rigidbodyの設定（既存のコードを維持）
            var rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }
        }
    }
}