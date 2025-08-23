using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.Animations;

public class ResultSceneManager : MonoBehaviour
{
    private GameManager gameManager;
    private CameraController cameraController;

    [Header("ResultCameraSetting")]
    [SerializeField] private Vector3 _resultCameraPosition = new Vector3();
    [SerializeField] private Vector3 _resultCameraRotation = new Vector3();

    [Header("Rank")]
    [SerializeField] private Vector3 _posRank1 = new Vector3();
    [SerializeField] private Vector3 _rotRank1 = new Vector3();
    [SerializeField] private Vector3 _posRank2 = new Vector3();
    [SerializeField] private Vector3 _rotRank2 = new Vector3();
    [SerializeField] private Vector3 _posRank3 = new Vector3();
    [SerializeField] private Vector3 _rotRank3 = new Vector3();
    [SerializeField] private Vector3 _posRank4 = new Vector3();
    [SerializeField] private Vector3 _rotRank4 = new Vector3();

    private void Start()
    {
        cameraController = FindFirstObjectByType<CameraController>();
        gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManagerが見つかりません。シングルトンが正しく設定されているか確認してください。");
            return;
        }

        StartCoroutine(SetupResultScene());
    }

    private IEnumerator SetupResultScene()
    {
        gameManager.ResultSetting();

        int maxAttempts = 10;
        int currentAttempt = 0;
        while ((gameManager.players == null || gameManager.players.Count == 0) && currentAttempt < maxAttempts)
        {
            yield return null;
            currentAttempt++;
        }

        if (gameManager.players == null || gameManager.players.Count == 0)
        {
            Debug.LogError("GameManagerのplayersリストが時間内に準備できませんでした。");
            yield break;
        }

        SetPlayerPositions(); // メソッド名を変更
        SetResultCameraMode();

        CrownScript crownScript = FindFirstObjectByType<CrownScript>();
        if (crownScript != null)
        {
            crownScript.CrownMove();
        }
    }

    public void SetResultCameraMode()
    {
        if (cameraController == null)
        {
            Debug.LogError("CameraControllerNULL");
            return;
        }
        cameraController.StopCameraMove();
        cameraController.ResultCameraMode(_resultCameraPosition, _resultCameraRotation);
    }

    /// <summary>
    /// playerの場所を決める
    /// </summary>
    /// <summary>
    /// 順位に応じてプレイヤーの位置と向きを設定します。
    /// </summary>
    public void SetPlayerPositions()
    {
        if (gameManager.players == null || gameManager.players.Count == 0)
        {
            Debug.LogWarning("GameManagerのplayersリストが空です。リザルトボールが生成されていません。");
            return;
        }

        // プレイヤーをRankプロパティの昇順（小さい値から大きい値へ）でソートします
        var sortedPlayers = gameManager.players.OrderBy(p => p.Rank).ToList();

        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            PlayerBase player = sortedPlayers[i];
            Vector3 newPosition;
            Quaternion newRotation;

            // ソートされたリストのインデックスに基づいて位置と回転を設定
            switch (i)
            {
                case 0: // 順位が1位のプレイヤー
                    newPosition = _posRank1;
                    newRotation = Quaternion.Euler(_rotRank1);
                    break;
                case 1: // 順位が2位のプレイヤー
                    newPosition = _posRank2;
                    newRotation = Quaternion.Euler(_rotRank2);
                    break;
                case 2: // 順位が3位のプレイヤー
                    newPosition = _posRank3;
                    newRotation = Quaternion.Euler(_rotRank3);
                    break;
                case 3: // 順位が4位のプレイヤー
                    newPosition = _posRank4;
                    newRotation = Quaternion.Euler(_rotRank4);
                    break;
                default:
                    Debug.LogWarning($"プレイヤーが多すぎます。インデックス {i} のプレイヤーは配置されません。");
                    continue;
            }

            // プレイヤーのTransformを更新
            player.transform.position = newPosition;
            player.transform.rotation = newRotation;
            player.transform.localScale = new Vector3(30f, 30f, 30f);

            // Rigidbodyの設定
            var rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }
        }
    }

}