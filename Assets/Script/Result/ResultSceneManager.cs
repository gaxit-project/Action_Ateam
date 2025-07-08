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
    /// ランクに応じてプレイヤー（ボール）のY座標を設定
    /// GameManagerのplayersリストに、リザルト用に生成されたボールが入っていることを前提とします。
    /// </summary>
    public void SetPlayerHeightsByRank()
    {
        if (gameManager.players == null || gameManager.players.Count == 0)
        {
            Debug.LogWarning("GameManagerのplayersリストが空です。リザルトボールが生成されていません。");
            return;
        }

        Debug.Log("PlayerSet");
        // リザルト表示用に、playersリストをランク順にソートします。
        // GameManagerのRankSort()はPlayerBaseオブジェクトのRankプロパティを更新しますが、
        // playersリスト自体の順序はソートしません。
        var sortedPlayersForDisplay = gameManager.players.OrderBy(p => p.Rank).ToList();


        float baseHeight = 10f; // 1位の高さ
        float step = 2f;        // 順位ごとの高さの差

        foreach (var player in sortedPlayersForDisplay) // ソートされたリストを順に処理
        {
            int rank = player.Rank;

            // Debug.Log($"Player: {player.name}, Rank: {rank}"); // デバッグ用に出力

            float y = baseHeight - (rank - 1) * step;
            if (y < 1f) y = 1f; // 最低の高さを設定（地面に埋まらないように）

            Vector3 pos = player.transform.position; // GameManagerで既に設定されたX, Z座標を維持

            player.transform.position = new Vector3(pos.x, y, pos.z);

            // Rigidbodyがある場合は、重力と物理演算を停止（GameManagerでも行っていますが、念のため）
            var rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }
        }
    }
}